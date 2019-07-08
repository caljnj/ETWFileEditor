# ETW File Editor

First ever project using C#

This project just take the excellen traceevent library and shows that you can chop up a file given start/end timestamp millis. 

The idea of this is as a step towards using Microsoft Message Analzyer (MMA). 

MMA has some great tooling but chokes with bigger files

- takes about 1 hour to load up an approx 300MB file
- UI slower than molasses


the workflow i would expect this tool to be useful is: 

- do your capture using perfview / or perfview command line
- click around in perfview till you find the timepoint of interest
- use ETWFileEditor to trim the file down to the few seconds before the time of interest
- load file into MMA and have a half-decent experience

