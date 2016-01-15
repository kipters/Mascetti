# Mascetti

It's a small library to provide translations on C#. It's inspired by 
[i18njs](http://i18njs.com/) and follows most of its concepts, but the string
interpolation format is different.

Features:

- Pluralization
- Support for C# format strings
- Multiple contexts

## Usage 

### Installation
Installation is done via Nuget, just: 
```
Install-Package Mascetti
```

Altought it isn't a requirement, localization files will usually be kept in JSON files. 
The Nuget package `Mascetti.Json` contains a few helpers for serializing, 
deserializing and validating such files. If you want to write your own 
serializer, a JSON schema is provided for validation.

### Initialization
Once you've got your `LanguageDefinition` object, you can instantiate a 
`LocalizedStringProvider`:
```
LanguageDefinition definition = ...
var m = new LocalizedStringProvider(definition);
```

You can also add more definitions like this:
```
LanguageDefinition otherDefinition = ...
m.Add(otherDefinition);
```

### Localization
Then, to get a localized string you can use the `Localize()` method:

```
m.Localize("profileUpdate");
```

this method returns a `LocalizedStringBuilder` that you can customize using a 
fluent API:

```
.WithContext(string context, string value);
.WithContexts(Dictionary<string, string> contexts);
```
Let's you define the context(s) to search into. Note that all the the 
context-value pair must match for a context to be selected!
If no matching context is found, the key will be searched in the default 
context. This for example let's you define two different contexts in
language that have a difference between male and female forms, and fallback to
a default one in languages that don't.

```
.WithParameters(...);
```
Lets you add parameters for string formatting, these are the object that will 
be used for replacing the placeholders. Note that this method is additive,
multiple calls will add parameters, not replace them! 

```
.Plural(int amount);
.Singular();
```
The first one lets you set the amount of "items" to localize for, so the
library can select the correct pluralization. For example you could have such a
definition (in JSON):
```
"comments": [
    [null, 0, "No comments"],
    [1, 1, "One comment"],
    [2, null, "{0} comments"]
]
```
And translate it like this:
```
mdef.Localize("comments")
    .Plural(numberOfComments)
    .WithParameters(numberOfComments);
```

I realize this is a pretty common scenario, and in such cases you can use
an overload of the `Localize()` method:
```
mdef.Localize("comments", numberOfComments);
```
in this case the second parameter will be used both for pluralization selection
and as first parameter for string formatting (the one that replaces `{0}`)
Of course if you need more parameters you can use `.WithParameters()` as usual.

`.Singular()` is equivalent to `.Plural(1)`

In the plurals definition, `null` is equivalent to "minus infinite" when it's 
used as `first` parameter and "plus infinite" when is used as `last`.

The first pluralization for which the expression
```
first ≤ x ≤ last
``` 
where `x` is the amount parameter, will be selected. This means that a 
definition such as `[null, null, "foobar"]` will **always** match

```
.ToString()
```
When you're done, just call this method to get the string representation you
selected. In most cases you won't really need this since the 
`LocalizedStringBuilder` is implicitly convertible to `string`.