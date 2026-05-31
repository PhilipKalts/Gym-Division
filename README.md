# Gym Division

This is a personal side-project where I showcase my abilities when it comes to backend developement.
Technologies used:
- Model-View-Controller (MVC)
- SQLite for storing, editing database
- Postman was used for testing endpoints

Gym Division is a web app designed with fitness trainers in mind. A gym has multiple members coming in per hour and the fitness trainer is tasked to quickly evaluate which gym member should go to each room. All that happens while new members may arrive last second or cancel their appointment. Furthermore, it is important to point out there may be multiple factors for the division of the members. Age, fitness level, injuries or pairs are some which may effect the result. The goal of this web app is to minimize the time a trainer needs to prepare the rooms down to mere seconds. By having all of the information of the rooms, selecting which members are in the gym at the time and lastly setting the parameters the trainer will get the best possible result.

## Databases
SQLite was used for the databases, one for the rooms and one for the members.

### Gym member
- Name
- Level
- Height
- Weight
- Age
- Notes

### Room
- Minimum members
- Maximum members
- Ideal members

## Division parameters
 - Fitness level
 - Age
 - Injuries
 - Pairs (some members may want to be in the same room with other members)

## Using the Web app 
The trainer has added all of the gym members and the rooms with their information in the databases via their appropriate pages. When it is time to generate the room, they navigate to the according page where the gym members can be selected. Afterwards, they can set the weight of each parameter and press "generate" which will return the results.

## How the system works:
There are 2 things which the system has to take care of:
1. How many members go on each room
2. Who goes where

### Room numbers
For deciding how many members enter the rooms we have to take into account the rooms data and how many members we have currently. The goal is to have all of the rooms filled with their ideal number. If that is not possible, the number increases or decreases based on the total number of members we have staying as close as possible with the ideal, while never decreasing below the minimum or exceeding the maximum.

### Members division
First, it is checked whether the user has entered only 1 parameter for the division to based upon. For example, if the only available parameter is the age the members can be sorted based on that and then add them in to the rooms.
However, if there are more than 1 parameter the algorithm creates proxy rooms and adds the members based on the order which the user has entered.
Each member has a score for that proxy room and each room has a score which is the sum of all the members inside it.
Afterwards, the members with the least score are being swapped with members from other rooms, while the overall score of both rooms. The system continues until no improvement can be made.
