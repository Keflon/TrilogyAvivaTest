# TrilogyAvivaTest

## Release Notes:

This sample app was written to demonstrate good architecture for an enterprise app.  
The requirements are:
- MVVM architecture
- Adhere to SOLID principles
- Demonstrate dependency-injection
- Include some unit tests
- Demonstrate persistence between application instances
- Make use of view lifecycle events

Libraries used:
- [SimpleInjector](https://www.nuget.org/packages/SimpleInjector)
  - For an IoC container
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)
  - REST comms serialisation / deserialisation
- [MvvmZero library](https://www.nuget.org/packages/FunctionZero.MvvmZero/2.2.5)
  - Page navigation and access to page lifecycle events
- [z:Bind](https://www.nuget.org/packages/FunctionZero.zBind)
  - To add view-logic without the need for lots of ValueConverters
- [CommandZeroAsync](https://www.nuget.org/packages/FunctionZero.CommandZero)
  - For ICommand instances, with the benefit of concurrent-execution guards, async commands and more

