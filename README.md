# Fluxor-persist

Fluxor-persist is a library to persist [Fluxor](https://github.com/mrpmorris/Fluxor) states.

## Introduction

Persisting states is a pretty common task .. in react I use [redux-persist](https://github.com/rt2zz/redux-persist) so this carries over some of the same ideas.  Most often I use it to ensure a user doesn't lose their state on a page refresh or when leaving or returning to a site but this library has no blazor dependencies so it can be used anywhere.

## Getting Started

### Installation
You can download the latest release / pre-release NuGet packages from the official Fluxor-persist nuget pages.

* [Fluxor.Persist](https://www.nuget.org/packages/Fluxor.Persist/) [![NuGet version (Fluxor.Persist)](https://img.shields.io/nuget/v/Fluxor.Persist.svg?style=flat-square)](https://www.nuget.org/packages/Fluxor.Persist/)

### Setup

The easiest way to get started is to look at the sample blazor app here(https://github.com/Tailslide/fluxor-persist)

To add Fluxor-persist to your existing blazor fluxor application you want to:


- Add a NuGet package reference to `Fluxor.Persist`
- Add `.UsePersist()` in Program.cs when building your existing Fluxor service with `AddFluxor()`
- Make a class that implements `IStringStateStorage` to persist your state however you want. It just needs to be able to save and retrieve a string / string key value pair. You can alternative implement `IObjectStateStorage` if you need to persist using the state objects directly.
- Add the following to your Program.cs to register both your state storage and the default persists json store handler:
```
builder.Services.AddScoped<IStringStateStorage, LocalStateStorage>();
builder.Services.AddScoped<IStoreHandler, JsonStoreHandler>();
```

    
### Detecting Rehydrate

You can detect that state has been rehydrated from storage. I use this in my MainLayout which inherits from FluxorLayout. In `OnInitialized` after I intialize the middleware to detect state restore and force a refresh:

```C#
        this.SubscribeToAction<InitializePersistMiddlewareResultSuccessAction>(result =>
        {
            Console.WriteLine($"**** State rehydrated**");
            this.StateHasChanged();// we now have state, we can re-render to reflect binding changes
        });
```


### Example State Storage Class for LocalStorage

```c#
using Blazored.LocalStorage;
using Fluxor.Persist.Storage;
using System.Threading.Tasks;

namespace Fluxor.Persist.Sample.Storage
{
    public class LocalStateStorage :IStateStorage
    {

        private ILocalStorageService LocalStorage { get; set; }

        public LocalStateStorage(ILocalStorageService localStorage)
        {
            LocalStorage = localStorage;
        }

        public async ValueTask<string> GetStateJsonAsync(string statename)
        {
            return await LocalStorage.GetItemAsStringAsync(statename);
        }

        public async ValueTask StoreStateJsonAsync(string statename, string json)
        {
            await LocalStorage.SetItemAsync(statename, json);
        }
    }
}
```


### Advanced Usage - BlackList, WhiteList

You can blacklist or whitelist state names to indicate if they should be persisted. Use only a blacklist or a whitelist not both.
Regardless of settings, the states @routing and PersistMiddleware are never persisted.

Examples: 

```c#
.UsePersist(x => x.SetBlackList(new string[] { "mystate1", "mystate2" }))
```

```c#
.UsePersist(options => options.UseInclusionApproach())
.UsePersist(x => x.SetWhiteList(new string[] { "mystate1", "mystate2" }))
```

### Advanced Usage - IPersist, ISkipPersist

Similarly, you can mark state classes to persit or not with `[IPersist]` and `[ISkipPersist]` attributes.
States can be not persisted by default by initializing with `.UsePersist(options => options.UseInclusionApproach())`

### Advanced Usage - Persist only some state properties

You can attribute your state records so that certain fields do not get serialized.
To not persist a 'isloading' flag for example:

```c#
    public record LoginState (
        [property: JsonIgnore] bool IsLoading,
        int CounterValue
        ) 
    {        
        [JsonConstructor]
        public LoginState(int CounterValue) : this(false, CounterValue) { }
    }
```

### BREAKING CHANGES IN 1.09

To convert from using persists pre-1.09 to 1.09+, you need the following changes:

This line can be removed from MainLayout and is no longer required:

`Dispatcher.Dispatch(new InitializePersistMiddlewareAction() { StorageService = new LocalStateStorage(this.localStorage), RehydrateStatesFromStorage=true });`

Your storage class that implements `IStateStorage` needs to change to use `IStringStateStorage`

The following two lines need to be added to Program.cs to register the default JSON handler and also your local storage class:

```c#
builder.Services.AddScoped<IStringStateStorage, LocalStateStorage>();
builder.Services.AddScoped<IStoreHandler, JsonStoreHandler>();
```

If you have a whitelist or a blacklist the method changed for setting them from:

`.UsePersist(x => x.StateBlackList= "mystate1,mystate2")`

to

`.UsePersist(x => x.SetBlackList(new string[] { "mystate1", "mystate2" }))`

