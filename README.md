# KendoUISignalRApp
Demo SignalR databinding for Telerik Kendo UI Grid with serverside sorting and filtering

!!! DO NOT USE THIS DEMO AS AN EXAMPLE FOR PRODUCTION PURPOSES. IT HAS A FEW PROBLEMS !!!

This is an example of a C# Asp.Net MVC5 application using a Telerik Kendo Grid that uses SignalR for data transport and synchronization.
The example uses the same database as the Telerik example, to be found at https://github.com/telerik/ui-for-aspnet-mvc-examples/tree/master/grid/signalR-bound-grid
You only have to change the connection string to a working database with the product samples.

Parts of the application are functioning, but there are some nasty problems. 
The SignalR part seems to work fine, but the filtering and sorting do not work as they should.

The problems: (having the application running as duplicate on two seperate browser screens, 1 and 2)
- when I add a new entry in screen 1, say 'Apple pie', I would expect the new entry to appear on top of the grid in screen 2. It doesn't; it appears at the bottom and stays there. It ignores the sorting.
- when a filter is active in screen 2, (for example, Product name contains 'Mutton') every new added product in screen 1 will appear in the grid in screen 2, ignoring the filtering. 
- related to the latter, every update in screen 1 will appear in screen 2, ignoring the filter.


