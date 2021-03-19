# Fluxor-persist

Fluxor-persist is a library to persist [Fluxor](https://github.com/mrpmorris/Fluxor) states.

## Introduction

Persisting states is a pretty common task .. in react I use [redux-persist](https://github.com/rt2zz/redux-persist) so this carries over some of the same ideas.  Most often I use it to ensure a user doesn't lose their state on a page refresh or when leaving or returning to a site but this library has no blazor dependencies so it can be used anywhere.

## Getting Started

## Installation
You can download the latest release / pre-release NuGet packages from the official Fluxor-persist nuget pages.

* [Fluxor.Persist](https://www.nuget.org/packages/Fluxor.Persist/) [![NuGet version (Fluxor.Persist)](https://img.shields.io/nuget/v/Fluxor.Persist.svg?style=flat-square)](https://www.nuget.org/packages/Fluxor.Persist/)

### Setup

The easiest way to get started is to look at the sample blazor app here(https://github.com/Tailslide/fluxor-persist)

To add Fluxor-persist to your existing blazor fluxor application you want to:


- Add a NuGet package reference to `Fluxor.Persist`
- Add `.UsePersist()` in Program.cs when building your existing Fluxor service with `AddFluxor()`
- Make a class that implements `IStateStorage` to persist your state however you want. It just needs to be able to save and retrieve a string / string key value pair.
- Override `OnInitialized` in your MainLayout to initialize your storage:

    ```C#
    Dispatcher.Dispatch(new InitializePersistMiddlewareAction() { StorageService = new LocalStateStorage(this.localStorage), RehydrateStatesFromStorage = true });
    ```
    
### Detecting Rehydrate

You can detect that state has been rehydrated from storage. I use this in my MainLayout which inherits from FluxorLayout. In `OnInitialized` after I intialize the middleware to detect state restore and force a refresh:

```C#
        Dispatcher.Dispatch(new InitializePersistMiddlewareAction() { StorageService = new LocalStateStorage(this.localStorage), RehydrateStatesFromStorage = true });
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
