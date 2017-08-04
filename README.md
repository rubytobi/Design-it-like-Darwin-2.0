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
The full data can be downloaded as [JSON-file](https://github.com/rubytobi/Design-it-like-Darwin-2.0/blob/master/readme/data.json).

The following is a brief example run with choosen setting as noted:

| Algorithm Settings | Bpm Settings |
| ------------------ | ------------ |
| Initial Genome: `<PI;AND(SEQ(a2;a5);SEQ(a4;XOR(v[1,5];a8;a7)));PO>` | Cost Weight: `1` |
| Maximum Number of Generations: `50` | Time Weight: `8000` |
| Population Size: `100` | Alpha: `0.01` |
| Tournament Size: `3` | i: `0.025` |
| Crossover Probability: `1` | Ifix: `0` |
| Mutation Probability: `0` | Ivar: `0` |
| Seed: `42` | T: `5` |
|  | N: `100` |
|  | Max Depth Random Genome: `5` |
|  | Only Valid Solutions: `off` |
|  | Only Valid Solutions At Start: `on` |
|  | Pain Factor: `10` |

On a virtual maschine in the cloud (t2.micro instance: 1 vCPU, 1 GiB RAM, ...), the following process returns as the best found after 22 sec.: `<PI;AND(SEQ(a1;a6);SEQ(a3;XOR(v[1,5];a8;a7)));PO>`

## Process Notation
We extended the notation used by Afflerbach et al. slightly. For start process declaration and for the developed process designs, please refer to this description:
* Process: `<PI; ... ;PO>`
* Gateways: `AND( ... ; ... )`, `SEQ( ... ; ... )`, `XOR(v[ ... , ... ]; ... ; ... )`. AND/SEQ have unlimited elements. XOR has a maximum of two elements. Within the brackets `v[ ... , ... ]` the decision variable (first) and the decision value (second) will be defined. 
* Tasks: `...`. Just use any name as defined in the data tab. Please refrain from using whitespaces.

## Limitations
* The number of exclusive gateways is limited to **1**.
* All data is treated as information. Hence there is no loosing of information possible or any constrains due to parallel access.

## Further Information
* [API-Documentation](https://github.com/rubytobi/Design-it-like-Darwin-2.0/wiki/API-Documentation)
