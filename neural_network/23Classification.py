# -*- coding: utf-8 -*-
"""
Created on Tue Jul 10 18:05:47 2018

@author: WFnian
"""

import numpy as np
import torch
from torch import nn, optim
from torch.autograd import Variable
import matplotlib.pyplot as plt


def retFloat(x):
    return float(x)


def subs(x):
    return x-1


def getData():
    f = open("F:\\kinect\\Kinect\\neural_network\\train_data.txt", 'r')
    data = f.readlines()
    y = []
    x = []
    for i in data:
        x.append(i.split()[:-1])
        y.append(i.split()[-1])
    res_x = []
    for i in x:
        r = map(retFloat, i)
        res_x.append(list(r))
    y = list(map(int, y))
    y = list(map(subs, y))

    return res_x, y


""" x 的形状类似于
x = [[],
     []
     []
     []]
len(x[0]) = 12
len(x) = 345

y 的形状类似于：表示分类
y = [0,
     1,
     2,
     3,
     ...
     ]
len(y) = 345
"""
x, y = getData()


class twentyclassification(nn.Module):
    def __init__(self, in_dim, n_hidden_1, n_hidden_2, out_dim):
        super(twentyclassification, self).__init__()
        self.layer1 = nn.Sequential(
            nn.Linear(in_dim, n_hidden_1), nn.ReLU(True))
        self.layer2 = nn.Sequential(
            nn.Linear(n_hidden_1,n_hidden_2 ), nn.ReLU(True))
        self.layer3 = nn.Sequential(nn.Linear(n_hidden_2, out_dim))

    def forward(self, x):
        x = self.layer1(x)
        x = self.layer2(x)
        x = self.layer3(x)

        return x


model = twentyclassification(14, 40, 50, 23)

criterion = nn.CrossEntropyLoss()
optimizer = optim.SGD(model.parameters(), lr=1e-3)

plt_loss = []
acc_ = []

for epoch in range(3000):
    x_data = Variable(torch.tensor(x))
    target = Variable(torch.tensor(y))
    #=================== forward ==================
    out = model(x_data)
    loss = criterion(out, target)
    #=================== backward =================
    optimizer.zero_grad()
    loss.backward()
    optimizer.step()
    # if epoch %1 ==0:
    plt_loss.append(loss.item())
    #===================accurate ==================
    vect = out.detach().numpy().tolist()
    acc = 0
    for i in range(len(vect)):
        if y[i] == vect[i].index(max(vect[i])):
            acc = acc+1
    acc = acc/len(vect)
    acc_.append(acc)


plt_loss = plt_loss[20:]

fig = plt.figure(0)
plt.figure(figsize=(3.64, 2.35))
plt.plot(list(range(len(plt_loss))), plt_loss, 'r')
plt.title("network loss")
plt.savefig('F:\\kinect\\Kinect\\neural_network\\train_loss_acc_pic\\loss.png')
plt.close(0)

fig = plt.figure(2)
plt.figure(figsize=(3.64, 2.35))
plt.plot(list(range(len(acc_))), acc_, 'g')
plt.title("network acc")
plt.savefig('F:\\kinect\\Kinect\\neural_network\\train_loss_acc_pic\\acc.png')
plt.close(2)

# model.eval()
torch.save(model, 'F:\\kinect\\Kinect\\neural_network\\23_classification_model.pth')

"""
"""

#predict = model(Variable(torch.tensor([[0,3,3,4,5,6,7,8]]).float()))
# print(predict)
