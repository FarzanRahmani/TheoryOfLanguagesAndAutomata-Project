class Variable:
    def __init__(self, LHS, RHS):
        self.LHS = LHS
        self.RHS = RHS


# 3
numOfVariables = int(input())

# <S> -> a<S>b | a<A> | b<B>
# <A> -> a<A> | #
# <B> -> b<B> | #
notHaveLambda = True
allVariables = []
for i in range(numOfVariables):
    ProductionRules = input()
    lhs = ProductionRules[1:2]
    r = ProductionRules.split("->")[1].strip()
    r2 = r.replace("<", "").replace(">", "").split("|")
    rhs = []
    for s in r2:
        z = s.strip()
        if (z == "#"):
            notHaveLambda = False
        rhs.append(z.replace("#", ""))
    allVariables.append(Variable(lhs, rhs))

allVariablesLHS = []
for v in allVariables:
    allVariablesLHS.append(v.LHS)

# aaab
inputString = input()

# Process
tmp = [allVariablesLHS[0]]
Found = False
count = 0
# while(count < 1100000):
#     for i in range(len(tmp)):
#         start = tmp[i]
#         if (start == inputString):
#             print("Accepted")
#             Found = True
#             break
#         for v in allVariablesLHS:
#             if (v in start):
#                 vRHS = allVariables[allVariablesLHS.index(v)].RHS
#                 flag = 0
#                 for r in vRHS:
#                     if(flag == 0):
#                         tmp[i] = start.replace(v,r,1)
#                         # tmp[i] = start.replace(v,r)
#                         if (tmp[i] == inputString):
#                             print("Accepted")
#                             Found = True
#                         flag += 1
#                     else:
#                         # if(len(start.replace(v,r,1)) < 2*len(inputString)): ###
#                         tmp.append(start.replace(v,r,1))
#                         # tmp.append(start.replace(v,r))
#                         if (tmp[-1] == inputString):
#                             print("Accepted")
#                             Found = True
#                             break
#                     # flag += 1
#                 break
#             if(Found):
#                 break
#         count += 1
#     if(Found):
#         break
if(notHaveLambda):
    while(count < 500000):
        for i in range(len(tmp)):
            start = tmp[i]
            for v in allVariablesLHS:
                if (v in start):
                    vRHS = allVariables[allVariablesLHS.index(v)].RHS
                    for r in vRHS:
                        # if(len(start.replace(v,r,1)) < 2*len(inputString)): ###
                        t = start.replace(v,r,1)
                        if (len(t) <= len(inputString)):
                            tmp.append(t)
                            if (tmp[-1] == inputString):
                                print("Accepted")
                                Found = True
                                break
                count += 1
                if(Found):
                    break
            tmp[i] = ""
        if(Found):
            break
else:
    while(count < 500000):
        for i in range(len(tmp)):
            start = tmp[i]
            for v in allVariablesLHS:
                if (v in start):
                    vRHS = allVariables[allVariablesLHS.index(v)].RHS
                    for r in vRHS:
                        # if(len(start.replace(v,r,1)) < 2*len(inputString)): ###
                        tmp.append(start.replace(v, r, 1))
                        if (tmp[-1] == inputString):
                            print("Accepted")
                            Found = True
                            break
                count += 1
                if(Found):
                    break
            tmp[i] = ""
        if(Found):
            break


if(not Found):
    print("Rejected")
