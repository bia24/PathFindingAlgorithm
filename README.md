# PathFindingAlgorithm
Implementations of some classical optimal path algorithms    
* Dijkstra : Shortest path search algorithm    
* BFS : Best-First-Search algorithm   
* A* : A-star algorithm    
## How to use？    
1. Clone or download this project. Open it in Unity.   
2. Open the scene named "PathFindingDemo",and run it.   
3. Show the path finding demo:  
   * Click "New Map" button can create a random map which has 50*75(fixed) cells with 1200(defalut)obstacles.   
   * Click "Set Start&End" button can entry "start&end choose" mode. When you click two cells in map on the screen(color will turn to red), this
   mode will exit.   
   * Click "Dijkstra"/"BFS"/"A Star" button, you can choose one algorithm to run this demo.    
## Result    
The results will be shown in the upper right:   
* Algorithm  :  the algorithm you choose to run;  
* Time :  the time (ticks) for one excute of this algorithm ;
* Searched Vertexes ： how many vertexes has been searched before finding the end;
* Path Length : how long the path from start to end this algorithm choosed.     

The color of cell means?
*  white :  this is a walkable cell;
*  grey ： this is a obstacle, impassable cell;  
*  red : the start cell and end cell;  
*  green : the vertexes (cells) have been searched;  
*  blue : the final path consists of walkable cells;
### Note
*  each cell can only move in four directions(up,down,left,right) with all same edge wights, just 1;
*  the priority_queue algorithms used is implemented by binary heap;
*  the Heuristic function algorithms used is implemented by Hamilton distance;

   
