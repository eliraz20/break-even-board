# Break Even Board

Compare profit for alternative prices, demand and production batch sizes.

## Try it

From this folder with the .NET 10 SDK installed:

```sh
dotnet run -- price study.tsv
```

Rows are scenario, selling price, demand, batch size, batch count, production cost per unit, setup cost and salvage value per leftover unit. Assumes unsold units realize the stated salvage value.

The included study.tsv uses tab-separated fields. Comment lines start with `#`.

The original exercise remains available with `dotnet run -- example.tsv`.

## When to use it

A student selling a batch of prints needs to choose a price and production quantity before ordering stock.

Example result:

```text
Standard: produced 100, sold 80, leftovers 20, unfilled 0, unit margin 6.00, profit 300.00
Premium: produced 100, sold 30, leftovers 70, unfilled 0, unit margin 11.00, profit -50.00
Best profit: Standard (300.00)
```

Related course or field workflow: [Ivey pricing simulation](https://www.iveypublishing.ca/s/product/marketplace-simulations-fundamentals-of-pricing/01tOF00000Dt3UNYAZ).
