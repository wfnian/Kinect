# -*- coding: utf-8 -*-
"""
Created on Thu Jul 12 09:02:49 2018

@author: WFnian
"""
import torch
from torch import nn, optim
from torch.autograd import Variable
import matplotlib.pyplot as plt
import sys


def retFloat(x):
    return float(x)


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


model = torch.load('F:\\kinect\\Kinect\\neural_network\\23_classification_model.pth')
# p = model(Variable(torch.tensor([409.947969373939,614.289002160591,1069.99326556048,729.388634304389,284.803383755373,392.435675163498,534.111825220838,1073.69154107929,1487.13061041836,233.262367480703,609.100048386539,451.452729308413])))
if len(sys.argv) > 1:
    pass_data = sys.argv[1].split(',')
    pass_data = list(map(retFloat, pass_data))
    # print(pass_data)
    # print(pass_data[:12])
    predict_pass = model(Variable(torch.tensor(pass_data)))
    # print(predict_pass)
    # print(predict_pass.max())
    c = predict_pass.detach().numpy().tolist()
    print(c.index(max(c))+1)
