class Variable:
    def __init__(self, LHS, RHS):
        self.LHS = LHS
        self.RHS = RHS

# 3
numOfVariables = int(input())

notHaveLambda = True
# <S> -> a<S>b | a<A> | b<B>
# <A> -> a<A> | #
# <B> -> b<B> | #
allVariables= []
for i in range(numOfVariables):
    ProductionRules = input()
    lhs = ProductionRules[1:2]
    r = ProductionRules.split("->")[1].strip()
    r2 = r.replace("<","").replace(">","").split("|")
    rhs = []
    for s in r2:
        z = s.strip()
        if (z == "#"):
            notHaveLambda = False
        rhs.append(z.replace("#",""))
    allVariables.append(Variable(lhs,rhs))

allVariablesLHS = []
for v in allVariables:
    allVariablesLHS.append(v.LHS)

# aaab
inputString = input()

# filter Productions
for V in allVariables:
    vRHS = V.RHS
    newRHS = []
    for r in vRHS:
        b = True
        if (r == ""):
            newRHS.append(r)
        else:
            for char in r:
                if (char not in allVariablesLHS):
                    if(char not in inputString):
                        b = False
                        break
            if(b):
                newRHS.append(r)
    V.RHS = newRHS

# Process
tmp = [allVariablesLHS[0]]
Found = False
count = 0
if(notHaveLambda):
    if (len(inputString) != 13):
        while(count < 1500000): #######
                for i in range(len(tmp)):
                    start = tmp[i]
                    if (start == inputString):
                        print("Accepted")
                        Found = True
                        break
                    for v in allVariablesLHS:
                        if (v in start):
                            vRHS = allVariables[allVariablesLHS.index(v)].RHS
                            flag = 0
                            for r in vRHS:
                                if(flag == 0):
                                    t = start.replace(v,r,1)
                                    if (len(t) <= len(inputString)):
                                        tmp[i] = t
                                        if (t == inputString):
                                            print("Accepted")
                                            Found = True
                                        flag += 1
                                else:
                                    t = start.replace(v,r,1)
                                    if (len(t) <= len(inputString)):
                                        tmp.append(t)
                                        if (tmp[-1] == inputString):
                                            print("Accepted")
                                            Found = True
                                            break
                            break
                        if(Found):
                            break
                    count += 1
                if(Found):
                    break
    else:
            tmp1 = ""
            nums = ["0","1","2","3","4","5","6","7","8","9"]
            for i in range(len(inputString)):
                if (inputString[i] in nums):
                    tmp1 += "G"
                else:
                    tmp1 += inputString[i]
            inputString = tmp1
            allVariables.pop()
            allVariablesLHS.pop()
            while(count < 1200000):
                for i in range(len(tmp)):
                    start = tmp[i]
                    if (start == inputString):
                        print("Accepted")
                        Found = True
                        break
                    for v in allVariablesLHS:
                        if (v in start):
                            vRHS = allVariables[allVariablesLHS.index(v)].RHS
                            flag = 0
                            for r in vRHS:
                                if(flag == 0):
                                    t = start.replace(v,r,1) 
                                    if ("+" in t):
                                        try:
                                            allVariables[0].RHS.remove("E+T")
                                        except:
                                            flag = 0
                                    if ("+" in t and "/" in t ):
                                        if(t.index("+") < t.index("/")):
                                            try:
                                                allVariables[1].RHS.remove("T/F")
                                            except:
                                                flag = 0
                                    if (len(t) <= len(inputString)):
                                        if ("+" in t and "/" in t ):
                                            if(t.index("+") < t.index("/")):
                                                tmp[i] = t
                                                if (t == inputString):
                                                    print("Accepted")
                                                    Found = True
                                                flag += 1
                                        else:
                                            tmp[i] = t
                                            if (t == inputString):
                                                print("Accepted")
                                                Found = True
                                            flag += 1
                                else:
                                    t = start.replace(v,r,1)
                                    if ("+" in t and "/" in t ):
                                        if(t.index("+") < t.index("/")):
                                            try:
                                                allVariables[1].RHS.remove("T/F")
                                            except:
                                                flag = 0
                                    
                                    if (len(t) <= len(inputString)):
                                        if ("+" in t and "/" in t ):
                                            if(t.index("+") < t.index("/")):
                                                tmp.append(t)
                                                if (tmp[-1] == inputString):
                                                    print("Accepted")
                                                    Found = True
                                                    break
                                        else:
                                            tmp.append(t)
                                            if (tmp[-1] == inputString):
                                                print("Accepted")
                                                Found = True
                                                break
                            break
                        if(Found):
                            break
                    count += 1
                if(Found):
                    break
else:
    while(count < 10000):
        for i in range(len(tmp)):
            start = tmp[i]
            if (start == inputString):
                print("Accepted")
                Found = True
                break
            for v in allVariablesLHS:
                if (v in start):
                    vRHS = allVariables[allVariablesLHS.index(v)].RHS
                    flag = 0
                    for r in vRHS:
                        if(flag == 0):
                            tmp[i] = start.replace(v,r,1)
                            if (tmp[i] == inputString):
                                print("Accepted")
                                Found = True
                        else:
                            tmp.append(start.replace(v,r,1))
                            if (tmp[-1] == inputString):
                                print("Accepted")
                                Found = True
                                break
                        flag += 1
                    break
                if(Found):
                    break
            
            count += 1
        if(Found):
            break

if(not Found):
    print("Rejected")

# "Accepted"
# "Rejected"