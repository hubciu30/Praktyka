import math
import matplotlib.pyplot as plt

# 1 - gracz
# 2 - stop
# 3 - blokada
map = \
    [
        [1, 0, 3, 0, 3, 0, 3, 0, 0, 0],
        [0, 0, 3, 0, 3, 3, 0, 0, 0, 0],
        [0, 0, 3, 0, 0, 0, 0, 0, 0, 0],
        [3, 0, 0, 0, 0, 3, 0, 0, 0, 0],
        [3, 3, 0, 3, 0, 3, 0, 0, 0, 0],
        [0, 0, 3, 3, 0, 3, 3, 0, 0, 0],
        [3, 0, 3, 2, 0, 0, 0, 3, 0, 3],
        [0, 0, 3, 0, 0, 0, 3, 3, 0, 0],
        [0, 3, 3, 0, 0, 0, 0, 0, 3, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    ]


class Cord():
    def __init__(self, x, y):
        self.X = x
        self.Y = y


class Vertice():
    def __init__(self):
        self.value = None
        self.position = None
        self.neighbours = []
        self.importance = None
        self.visited = False

    def AddNeighbour(self, vert):
        self.neighbours.append(vert)


class Graph():
    def __init__(self):
        self.vertices = []

    def AddVertice(self, item):
        self.vertices.append(item)


def GetStartCord(map):
    x = 0
    y = 0
    for ox in map:
        y = 0
        for oy in ox:
            if oy == 1:
                return Cord(x, y)
            y = y + 1
        x = x + 1


def GetGoalCord(map):
    x = 0
    y = 0
    for ox in map:
        y = 0
        for oy in ox:
            if oy == 2:
                return Cord(x, y)
            y = y + 1
        x = x + 1

def GetDistanse(C1, C2):
    return math.sqrt((math.pow(C2.X - C1.X, 2))+(math.pow(C2.Y - C1.Y, 2)))

def GetNeighbours(map, cord):
    NeiVert = []

    def check(map, cord):
        if cord.X >= 0 and cord.Y >= 0 and cord.X < len(map) and cord.Y < len(map[0]):
            return True
        else:
            return False

    if check(map, Cord(cord.X + 1, cord.Y)):
        NeiVert.append(map[cord.X + 1][cord.Y])
    if check(map, Cord(cord.X - 1, cord.Y)):
        NeiVert.append(map[cord.X - 1][cord.Y])
    if check(map, Cord(cord.X, cord.Y + 1)):
        NeiVert.append(map[cord.X][cord.Y + 1])
    if check(map, Cord(cord.X, cord.Y - 1)):
        NeiVert.append(map[cord.X][cord.Y - 1])

    return NeiVert


def GenerateGraph(map):
    graph = Graph()
    tempMap = []
    Stop = GetGoalCord(map)
    for x in range (len(map)):
        temp = []
        for y in range(len(map[x])):
            vertice = Vertice()
            vertice.position = Cord(x, y)
            if vertice.position.X == GetStartCord(map).X and vertice.position.Y == GetStartCord(map).Y:
                vertice.visited = True
            vertice.value = map[x][y]
            vertice.importance = GetDistanse(vertice.position, Stop)
            temp.append(vertice)
        tempMap.append(temp)

    for x in range(len(tempMap)):
        for y in range(len(tempMap[x])):
            nei = GetNeighbours(tempMap, Cord(x, y))
            for i in nei:
                if i.value != 3:
                    tempMap[x][y].AddNeighbour(i)
            graph.AddVertice(tempMap[x][y])
    return graph

def GetNotVisited(nei):
    clean = []
    for i in nei:
        if i.visited is False:
            clean.append(i)
    return clean

def Run(Start, Stop):
    History = []
    Current = Start
    #History.append(Current)
    while not Current.importance == Stop.importance:
        min = None
        neighbours = GetNotVisited(Current.neighbours)
        if len(neighbours) > 0:
            for nei in neighbours:
                if min is None or nei.importance < min.importance:
                    Current = nei
                    min = nei
            Current.visited = True
            History.append(Current)
        else:
            History.pop(len(History)-1)
            if len(History) == 0:
                return History
            else:
                Current = History[len(History)-1]
    History.pop(len(History)-1)
    return History


def PrintPath(map, history):
    Droga = []
    for x in map:
        temp = []
        for y in x:
            temp.append(' ')
        Droga.append(temp)

    for z in history:
        Droga[z.position.X][z.position.Y] = '*'

    for i in Droga:
        print(i)
    print("\n")


for i in map:
    print(i)

print("\n")

graph = GenerateGraph(map)
Start = None
Stop = None
for i in graph.vertices:
    if i.value == 1:
        Start = i
    elif i.value == 2:
        Stop = i


History = Run(Start, Stop)

print("\n")

if len(History) > 0:
    PrintPath(map, History)
else:
    print("Odp:\tBrak drogi!")

for i in History:
    map[i.position.X][i.position.Y] = 5
plt.imshow(map)
plt.colorbar()
plt.show()
'''
for i in graph.vertices[1].neighbours:
    print(i.position.X, "  ", i.position.Y, " -> ", i.visited)
'''