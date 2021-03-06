# Marital Age Probability

## What is that?

Under Marital Age probability we speak of the age when a person decides to
perform a life event such as marrying. When a person is of age they have the right
to decide to marry. 

## How is it decided?

We have gatherd the information about the ages, genders and marital status of the
citizens in The Netherlands from CBS. With this data we made a construction to take
those weights for marital status on ages, with the current age list of citizens we have.
In this age list we have the current amount of citizens and the current ages available in
it.

## What data was used?

[Bevolking; geslacht, leeftijd en burgerlijke staat, 1 januari
](https://opendata.cbs.nl/statline/?dl=308BE#/CBS/nl/dataset/7461bev/table):
From this table we found the information we used for the weights. In this table we can find
the Marital Status and the age. Unfortunately we did not find the data for registerd Partnerships,
so we left it out in this.

[Prognose bevolking; geslacht en leeftijd, 2020-2060
](https://opendata.cbs.nl/statline/#/CBS/nl/dataset/84646NED/table?ts=1605714142946):
From this table we used the age calculations of the citizens during the year 2020. This is
the list of generated citizens in the code.

## Graph Simulated Data
![alt text](./img/Graph_Simulated_MarriageVsSingle.png)

In the Graph above you can see that the peak age in the simulated data is around 54 years old.
You can also see that the amount of single woman is at it's highest the minute they turn
18 years old. This is because most teenagers don't decide to marry at a young age.

In this graph there is also 'missing' information. This is because we decided
that one first has to become married before they divorce or become a widow/widower. In
this graph the only thing you see is when they decide to tie the knot.
 
#### A graph inculding the dicorc�s/divorc�es
![alt text](./img/SimulatedMaritalAge_Graph.png)

In the Graph above you see that the peak in Marital Age is lower around the 54 year old age
range. This is because we added the cases of divorced citizens into this one so that
we could have a more acurate representation.

### Is the data similar?

![alt text](./img/MarriageVsSingle_CBS-Simulated_Graphs.png)

In the left table you can find the graph of the data we found on the site of CBS.
The data in here is the data that we used as our weight list to see if 
someone would marry or not. The right one show the data made by the simulation.

As you can see the graphs are almost identical in form. The only difference is
that because we only took the status of being married or being single, the graph contains
people who originally belong under the status of divorc�/dicorc�e or widow/widower.


#### Graphs inculding dicorc�s/divorc�es
![alt text](./img/GraphCBSvsSimulated_MaritalAge.png)

In this image on the left table you can see the data that we took from CBS, inculding the dicorc�s/divorc�es.
As you can see compared to the image above, the peak is alot lower. And when compared to 
the new data side by side you can see that the images look almost identical to eachother.

The reason we think that the images are fully correct is because we left out our widows/widowers,
because that is based on a life event which is not really a predicatable event.

### Code Showcase
```csharp
for (int i = 18; i < 100; i++) 
            {
                womenList.Add(i, new List<PartnerTypeAge>());
                menList.Add(i, new List<PartnerTypeAge>());

                for (int j = 0; j < femaleAge[i]; j++) 
                {
                    weights = frame.GetColumn<double>(Convert.ToString(i)).Values.Select(c => Convert.ToDouble(c)).Skip(3).Take(3).ToList();
                    MaritalStatusAge.Weights = weights;
                    womenList[i].Add(env.RandChoice(MaritalStatusAge.Source, MaritalStatusAge.Weights));
                }

                for (int j = 0; j < maleAge[i]; j++) 
                {
                    weights = frame.GetColumn<double>(Convert.ToString(i)).Values.Select(c => Convert.ToDouble(c)).Take(3).ToList();
                    MaritalStatusAge.Weights = weights;
                    menList[i].Add(env.RandChoice(MaritalStatusAge.Source, MaritalStatusAge.Weights));
                }

                collection.Add(new MaritalAgeProbability()
                {
                    SingleWomen = womenList[i].Where(p => p == PartnerTypeAge.Single).Count(),
                    MarriedWomen = womenList[i].Where(p => p == PartnerTypeAge.Married).Count(),
                    DivorcedWomen = womenList[i].Where(p => p == PartnerTypeAge.Divorced).Count(),
                    SingleMen = menList[i].Where(p => p == PartnerTypeAge.Single).Count(),
                    MarriedMen = menList[i].Where(p => p == PartnerTypeAge.Married).Count(),
                    DivorcedMen = menList[i].Where(p => p == PartnerTypeAge.Divorced).Count(),
                    Age = i
                });
            }
```

In the code above you can see that we started with a for loop, we did this because
we knew that we were only after the data of age 18 to 99, as that were the ages we had data for.
Continuing, you can see that we added a new "List(PartnerTypeAge)" to womenList and menList,
because these 2 values are dictionaries.

You can see that are 2 different loops within the bigger for loops. This is so that
we could loop through the ages in both the male and female age list. As both of them have
different amounts of people in it.

At the end you can see that we start to split the data up and count them. We count the amount
of women who are either married or still single and the same goes for the men. Once this has
happend for all ages we exported the collection into a CSV file and with the
results in that file we made the Graphs you have seen earlier.

## Simulation Run First Results
![alt text](./img/SimulationResults_TestRun_MarriedPerYear.png)

In the graph above you can see 2 different tables. The table on the left shows
the data that has been generated where the weights are 1:1:1, and on the other side we have
weights that are 1873469:166220:1. With both graphs you can see that the first
married pairs are only started to form after a few years. 

The first people born in the simulation are all born in 1920, while in both graphs
the first married couples appear only by 1949/1951. This means that once they enter
adulthood they do not immediately go and get coupled up to get married.

This is proof to show that the Age Weights work properly in the simulation. This
means that we can rely properly on the MaritalAgeProbability numbers.