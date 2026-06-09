# Gym Division

This is a personal side-project where I showcase my abilities when it comes to backend developement.
Technologies used:
- Model-View-Controller (MVC)
- SQLite for the databases
- Postman was used for testing the endpoints

Gym Division is a web app designed with fitness trainers in mind. A gym has multiple members coming in per hour and the fitness trainer is tasked to quickly evaluate which gym member should go to each room. Furthermore, it is important to point out there may be multiple factors for the division of the members. Age, fitness level, injuries or pairs are some which may effect the result. The goal of this web app is to minimize the time a trainer needs to finalize the room members down to mere seconds. By having all of the information of the rooms, selecting which members are in the gym at the time and lastly setting the parameters the trainer will get the best possible result.

## Databases
SQLite was used for the databases, one for the rooms and one for the members.

### Gym member Data
- Name
- Level
- Height
- Weight
- Age
- Notes

### Room Data
- Minimum members
- Maximum members
- Ideal members

## Division parameters
 - Fitness level
 - Age
 - Injuries
 - Pairs of members (some members may want to be in the same room with other members)
Worth pointing out, each parameter may have different weight to it. The user has the freedom to adjust the weight of each parameter with a slider.

## Using the Web app 
The trainer has added all of the gym members and the rooms with their information in the databases via their appropriate pages. When it is time to generate the room, they navigate to the appropriate page where the gym members can be selected. Afterwards, they can set the weight of each parameter and press "generate" which will return the results.

## How the system works:
There are 2 things which the system has to take care of:
1. Room distribution: how many members go on each room
2. Room populator: populating the rooms with the appropriate members


## Room Distribution Algorithm
The system is responsible for distributing gym members across multiple rooms while respecting capacity constraints and optimizing toward each room’s ideal size.
Each room defines:
- Minimum capacity
- Ideal capacity
- Maximum capacity
The algorithm operates in three distinct scenarios depending on total demand.


### 1. Over Ideal Capacity
When total members exceed the sum of all ideal room capacities:
- Each room is initially filled up to its ideal capacity
- Remaining members are distributed iteratively across rooms
- Distribution continues in a round-robin manner while respecting each room’s maximum capacity
This ensures balanced overflow without overloading any single room.


### 2. Between Minimum and Ideal Capacity
When total members fall between the sum of minimum and ideal capacities:
- Each room is initialized at its minimum capacity
- Remaining members are distributed toward each room’s ideal capacity
- Allocation stops when either all members are assigned or all rooms reach their ideal values


### 3. Below Minimum Capacity Constraint
When total members are fewer than the combined minimum required:
- Rooms are filled sequentially until available members are exhausted
- Only rooms that can satisfy their minimum requirement are assigned members
- Remaining members are distributed where possible under maximum constraints


## Room populator Algorithm (Local Search Heuristic)
After initial member assignment, the system performs an optimization phase to improve room quality based on a weighted compatibility score.
Each room maintains a global score, defined as the sum of pairwise compatibility between all members in the room.


### Objective Function
The system aims to maximize compatibility between members inside the same room based on weighted factors such as:
  - Age similarity
  - Fitness level similarity
  - Injury compatibility
  - Predefined pair preferences
Each factor contributes to a final weighted score per member pair.
The compatibility score is computed as a weighted sum of pairwise evaluations between members.


### Optimization Strategy
The system applies a local search heuristic:
1. Rooms are evaluated and sorted by internal member scores
2. The lowest-performing members in a room are selected as swap candidates
3. Potential swaps are evaluated against members in other rooms
4. A swap is accepted only if it improves the combined room score
5. The process repeats until no improving swaps are found, or maximum iteration limit is reached


### Swap Evaluation
For each potential swap:
- The system calculates the delta in total room score
- Only swaps with positive improvement are considered
- The best improvement across all candidates is selected per iteration
This ensures a best-improvement greedy strategy.
