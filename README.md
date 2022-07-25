# TrilogyAvivaTest

## Release Notes:
**TODO:** 
- Add some unit-tests to the stubbed test project. **(Required)**
- Add a splash screen. **(Required)**
- Make at least some effort to make the UI pretty?

This codebase represents 8 hours work (including breaks) from `File->New …`
-	Unit tests and a splash screen can be added tomorrow  
  - Apart from `Newtonsoft.Json` and `SimpleInjector`, all additional (i.e. non-boilerplate) NuGet packages are authored by myself.  

The App adheres to **SOLID** principles throughout, is fully **MVVM** and fully asynchronous
- IoC achieved using SimpleInjector. See [Startup.cs](https://github.com/Keflon/TrilogyAvivaTest/blob/main/TrilogyAvivaTest/TrilogyAvivaTest/Bootstrap/Startup.cs)
  - Even the App instance is created by the container!
- there is no tight-coupling between View and ViewModel.
- Interfaces are used where appropriate rather than in a cargo-cult fashion.
- The Views have no code-behind.
- Pages, PageViewModels, ViewModels, Models and Services are in clearly defined namespaces
  - It would be trivial to separate them out into loosely coupled libraries.
- Models are immutable! They are not wrapped in a ViewModel, though that’s trivial if it became required
- Async all the way. There is no blocking code on the UI thread.

The UI is ‘white label’ ready to be styled. The app is written to demonstrate good architecture, not pretty UI!   
There is no platform-specific code apart from getting the App instance reference.  
Navigation is performed by the [MvvmZero library](https://www.nuget.org/packages/FunctionZero.MvvmZero/2.2.5). I wrote that library :) 

The XAML makes use of a custom Binding extension called [z:Bind](https://www.nuget.org/packages/FunctionZero.zBind) to avoid the need for 
ValueConverters (it can do much more). I wrote that too. See [HomePage.xaml](https://github.com/Keflon/TrilogyAvivaTest/blob/main/TrilogyAvivaTest/TrilogyAvivaTest/Mvvm/Pages/HomePage.xaml) and [CityWeatherPage.xaml](https://github.com/Keflon/TrilogyAvivaTest/blob/main/TrilogyAvivaTest/TrilogyAvivaTest/Mvvm/Pages/CityWeatherPage.xaml)

Pages and PageViewModels are singletons.
-	Any transient PageViewModel ‘state’ is injected when the corresponding page is pushed.

The Rest service contains a singleton `HttpService`, so we can’t suffer from ‘port starvation’.  
The API layer ([OpenWeatherService.cs](https://github.com/Keflon/TrilogyAvivaTest/blob/main/TrilogyAvivaTest/TrilogyAvivaTest/Services/Api/OpenWeatherService.cs), …) sits between the low-level Rest service and the application layer.
-	The app does not interact directly with the Rest layer.

## Extras:
If you mis-spell a location and the API finds a suitable location, the spelling is corrected, and if the (mis-spelt) location has been saved, 
the saved location is updated.  
A saved location can be deleted.  
The app counts how many times it has been cold-started and says ‘Hi’ on first run.
-	You can reset this count on the HomePage.

Buttons disable, hide and/or update their text automatically, as appropriate.
-	Mostly using [CommandZeroAsync](https://www.nuget.org/packages/FunctionZero.CommandZero). I wrote that too.  

If you find any bugs or have any requests let me know and I'll fix them! :)


