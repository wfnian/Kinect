# -*- coding: utf-8 -*-
"""
Created on Tue Jul 10 18:05:47 2018

@author: WFnian
"""

import numpy as np
import torch
from torch import nn,optim
from torch.autograd import Variable
import matplotlib.pyplot as plt

def retFloat(x):
    return float(x)
def subs(x):
    return x-1

def getData():
    f = open("F:\\kinect\\11.txt",'r')
    data = f.readlines()
    y = []
    x = []
    for i in data:
        x.append(i.split()[:-1])
        y.append(i.split()[-1])
    res_x = []
    for i in x:
        r = map(retFloat,i)
        res_x.append(list(r))
    y = list(map(int,y))
    y = list(map(subs,y))
  
    return res_x,y

getData()
x,y = getData()

class twentyclassification(nn.Module):
    def __init__(self,in_dim,n_hidden_1,n_hidden_2,out_dim):
        super(twentyclassification,self).__init__()
        self.layer1=nn.Sequential(nn.Linear(in_dim,n_hidden_1),nn.ReLU(True))
        self.layer2=nn.Sequential(nn.Linear(n_hidden_1,n_hidden_2),nn.ReLU(True))
        self.layer3=nn.Sequential(nn.Linear(n_hidden_2,out_dim))
    
    def forward(self,x):
        x=self.layer1(x)
        x=self.layer2(x)
        x=self.layer3(x)
        
        return x
model = twentyclassification(12,400,200,23)

#if torch.cuda.is_available():
#    model = model.cuda()

criterion = nn.CrossEntropyLoss()
optimizer = optim.SGD(model.parameters(),lr=1e-2)

plt_loss = []
acc_ = []

for epoch in range(5000):
    x_data = Variable(torch.tensor(x))
    target = Variable(torch.tensor(y))
    #=================== forward ==================
    out = model(x_data)
    loss = criterion(out,target)    
    #=================== backward =================
    optimizer.zero_grad()
    loss.backward()
    optimizer.step()
    if epoch %1 ==0:
        plt_loss.append(loss.item())
    #===================accurate ==================
    
    vect = out.detach().numpy().tolist()
    acc = 0
    for i in range(len(vect)):
        if y[i]==vect[i].index(max(vect[i])):
            acc = acc+1
    acc = acc/len(vect)
    acc_.append(acc)       
    #break
    
plt_loss = plt_loss[20:]
#print(plt_loss)

plt.subplot(211) 
plt.plot(list(range(len(plt_loss))),plt_loss,'r')
plt.title(u"loss")
plt.subplot(212) 
plt.plot(list(range(len(acc_))),acc_,'g')
plt.title(u"acc")
model.eval()



#predict = model(Variable(torch.tensor([[0,3,3,4,5,6,7,8]]).float()))
#print(predict)