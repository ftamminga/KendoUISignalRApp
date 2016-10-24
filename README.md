# KendoUISignalRApp
Demo SignalR databinding for Telerik Kendo UI Grid with serverside sorting, filtering and paging

This is a working example of a C# Asp.Net MVC5 application with a Telerik Kendo Grid that uses SignalR for data transport and synchronization.
The examples from Telerik fail when using server side filtering and sorting out of the box. This example shows how to address the issues without too much problems. It only takes a small amount of work server side and a bit of javascript client side to make it work flawlessly. 

Just open the application in two or more browser screens and play with some sorting, filtering. Add some entries, change them. 

A short video of the basics can be seen here: https://goo.gl/m0hfmJ
It shows two synchronised browser screens. What's changing in one screen will reflect in the other. 

The example uses the same database as the Telerik examples, to be found at https://github.com/telerik/ui-for-aspnet-mvc-examples/tree/master/grid/signalR-bound-grid
You only have to change the connection string to a working database with the product samples.




