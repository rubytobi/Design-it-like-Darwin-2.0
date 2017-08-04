# Design-it-like-Darwin-2.0
**Abstract.** Process redesign is an important and valuable phase of the business process management (BPM) lifecycle. However, human creativity and objectiveness regarding continuous redesign initiatives are limited and biased. To overcome these limitations, we propose computational support based on evolutionary algorithms. Our software tool extends a formerly published proof of concept. Novelties have been introduced by including a new data structure, new mutation and crossover operators as well as an extended evaluation of unambiguous process designs explicitly considering time objectives. Finally, the tool provides new computation process (re-)design support to the BPM community.

**Keywords:** business process management, process redesign, computational in-telligence, evolutionary algorithm

## Screencast
This screencast explains the various UI elements and how to handle the tool. The screencast ist available on [YouTube](https://www.youtube.com/watch?v=vTG7PFUg3Rg).
[![YouTubeScreencast](/readme/youtube.png)](https://www.youtube.com/watch?v=vTG7PFUg3Rg)


## List of currently supported BPMN Elements
* **Activities:** Tasks
* **Gateways:** Exclusive, Parallel, (Sequential)
* **Data:** Data Object, Data Input, Data Output

## Example
The example is used by [Afflerbach et al. (2016)](https://doi.org/10.1007/s10796-016-9715-1) and inspired by [Vergidis et al. (2007)](https://doi.org/10.1016/j.ijpe.2006.12.032) and relates to a real travel agent process.

## Limitations
* The number of exclusive gateways is limited to **1**.
* All data is treated as information. Hence there is no loosing of information possible or any constrains due to parallel access.

## Further Information
* [API-Documentation](https://github.com/rubytobi/Design-it-like-Darwin-2.0/wiki/API-Documentation)
