Personal Finances
============
![](https://i.imgur.com/N98iHKU.png)

## About
Personal Finances is a simple personal finances managament system. With it you can manage your finances inserting your expenses and your savings in a custom created account and divide them by category and by subcategory. If you want, you also can aggregatte your movements by project, where you can define a end date and a budget if necessary. You also are able to get reports where you can filter by account, category, project, accounting date and others.

## Features
* Debit and credit movements insertion
* Transfers between accounts
* Custom categories and subcategories
* Custom Projects
* Custom accounts
* Analytical view (total credit, balance, balance on month) of accounts and categories
* Reports

## Changelog
* 1.0.1 - 02/21/2019
  * Performance increase
  * UI adjusts
  * New features: projects and reports
 
* 1.0.0 - 01/21/2019
  * First version of project

## Technologies
* Front-end
  * HTML, CSS and JavaScript (views are rendered with Razor engine)
* Back-end
  * C#
* Data access
  * Entity Framework 6 (with LINQ expressions)
* Frameworks
  * ASP.NET MVC 5.2
* Plugins and Libs
  * Bootstrap 3.3.7
  * jQuery 3.3.1
  * DataTables 1.10.18
  * Bootstrap Datepicker 
  * jQuery Mask 1.14.15
* Others
  * [AdminLTE](https://github.com/almasaeed2010/AdminLTE) (app template)
 
 ## Installation guide
  * Dependencies
     * A relational database like SQL Server, SQLite, MySQL or PostgreSQL (go to [Entity Framework 6 Providers](https://docs.microsoft.com/pt-br/ef/ef6/fundamentals/providers/) to see the supported providers)
  * Clone this repository `https://github.com/carlosdaniiel07/personal-finances.git`
  * Configure your connection string on `Web.config` (see [Connection Strings Reference](https://www.connectionstrings.com/) if you need help)
  * Generate a database with the command `Update-Database` on Package Manager Console
  * Build and run the application
