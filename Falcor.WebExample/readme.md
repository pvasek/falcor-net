# Falcor.NET Example

This is the OWIN example for Falcor.NET.

[falcor url: model.json](model.json)

# Falcor examples

**Simple property**

Model path: `Events[0].Name` 

Falcor path: `['Events',0,'Name']` 

[Try it](model.json?path=['Events',0,'Name'])

**Multiple simple properties**

Model path: `Events[0].Name, Events[0].Number` 

Falcor path: `[['Events',0,'Name'],['Events',0,'Number']]` 

[Try it](model.json?path=[['Events',0,'Name'],['Events',0,'Number']])

# Model definition

[Model](Model.cs)