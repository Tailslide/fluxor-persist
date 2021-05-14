# Contributing

## A Guide to Contributing to Fluxor-Persist
Thanks for joining the team !  Please follow these guidelines to speed inclusion of your changes. 

## Design 
When creating changes, please think of an elegant way of incorporating them that emphasizes:

1. Backwards Comatibility
2. Clean Intuitive code for the library users

## Fork-and-pull Git workflow
In general, we follow the "fork-and-pull" Git workflow.
1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Push your work back up to your fork
4. Commit changes to your own branch
5. Submit a Pull request so that we can review your changes.

**Be sure to rebase on top of the latest from "upstream" before making a pull request!**

It is always possible to request a topic branch, with one if the team members. They will create one for you, which can be used instead of your local fork. The rest of the workflow will be exactly the same, with the benifit of having more users to test your code.

### Logging
Please follow the existing code examples of using the Microsoft logging library if you want to write to log.

```c#
using Microsoft.Extensions.Logging;

private ILogger<MyClass> _logger;
_logger?.LogDebug($"Hi There {username}");
```

