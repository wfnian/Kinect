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


def getData():
    pass

x = [
     [1,2,3,4,5,6,7,8],
     [2,3,4,5,6,7,8,9],
     [3,4,5,6,7,8,9,10],
     [4,5,6,7,8,9,10,11],
     [5,6,7,8,9,10,11,12],
     [6,7,8,9,10,11,12,13]
     ]
y = [
     0,
     1,
     2,
     3,
     4,
     5
     ]

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
model = twentyclassification(8,5,70,6)

criterion = nn.CrossEntropyLoss()
optimizer = optim.SGD(model.parameters(),lr=1e-2)

plt_loss = []

for epoch in range(5000):
    x_data = Variable(torch.tensor(x).float())
    target = Variable(torch.tensor(y))
    #=================== forward ==================
    out = model(x_data)
    loss = criterion(out,target)
    
    #=================== backward =================
    optimizer.zero_grad()
    loss.backward()
    optimizer.step()
    if epoch %50 ==0:
        plt_loss.append(loss.item())

model.eval()
predict = model(Variable(torch.tensor([[0,3,3,4,5,6,7,8]]).float()))
print(predict)