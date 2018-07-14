# Kinect应用-结合神经网络的姿势识别
_Kinect for detecting postures of chinese kongfu..._


<!-- TOC -->

- [Kinect应用-结合神经网络的姿势识别](#kinect应用-结合神经网络的姿势识别)
    - [1.1. 项目进度](#11-项目进度)
    - [1.2. Tips](#12-tips)
    - [1.3. 基于神经网络的分类](#13-基于神经网络的分类)
        - [1.3.1. 分类有问题啊。](#131-分类有问题啊)
        - [1.3.2. 又出现奇迹了。](#132-又出现奇迹了)
        - [1.3.3. 调整了学习率以及神经单元的层数](#133-调整了学习率以及神经单元的层数)
    - [1.4. 接下来几日日程安排](#14-接下来几日日程安排)
    - [1.5. 注意事项](#15-注意事项)

<!-- /TOC -->

## 1.1. 项目进度![Build Status](https://img.shields.io/badge/%E8%BF%9B%E5%BA%A6-Stagnation-lightgrey.svg)

___ 



- [x] 
1.太极姿势集合。  见pic.jpg ,最好会做。![Build Status](https://ci.pytorch.org/jenkins/job/pytorch-master/badge/icon) ![Status](https://img.shields.io/badge/finished-%E9%99%88%E8%BF%9C%E5%86%9B%2C%E4%BD%95%E5%BD%A6%E4%BD%B6-blue.svg) ![node](https://img.shields.io/badge/%E7%8E%8B%E6%96%B9%E5%B9%B4-adding-green.svg)

- [ ] 
2.如何分类？ ①.svm支持向量机。 ②.普通分类？可以算两个节点之间的距离（二范数、曼哈顿距离）③.深度学习方法![Pytorch](https://img.shields.io/badge/Framework-PyTorch-brightgreen.svg)  ![node](https://img.shields.io/badge/%E7%8E%8B%E6%96%B9%E5%B9%B4-adding-green.svg)


- [x] 
3.数据库入库。![Build Status](https://camo.githubusercontent.com/7ff1a64ca6e9f85bcdfc81a2e11bff01b9ad3d33/68747470733a2f2f7472617669732d63692e6f72672f70696b65736c65792f6769746875626261646765732e737667) ![node](https://img.shields.io/badge/%E7%8E%8B%E6%96%B9%E5%B9%B4-adding-green.svg) ![finfshed](https://img.shields.io/badge/finished-%E9%99%88%E8%BF%9C%E5%86%9B-blue.svg)
- [x] 
4.20个点标记。  ![Build Status](https://ci.pytorch.org/jenkins/job/pytorch-master/badge/icon) ![node](https://img.shields.io/badge/%E7%8E%8B%E6%96%B9%E5%B9%B4-adding-green.svg)
- [x] 
5.rgbd深度摄像机点集获取。  ![Build Status](https://camo.githubusercontent.com/7ff1a64ca6e9f85bcdfc81a2e11bff01b9ad3d33/68747470733a2f2f7472617669732d63692e6f72672f70696b65736c65792f6769746875626261646765732e737667) ![node](https://img.shields.io/badge/%E7%8E%8B%E6%96%B9%E5%B9%B4-adding-green.svg) 

## 1.2. Tips 
___
- 普通分类：获取相应关节点的坐标计算距离

[基于kinect的人体动作识别系统](https://img-blog.csdn.net/20170930162524582?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvYmFvbGlucQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/Center)

## 1.3. 基于神经网络的分类
loss1 | loss2(抛弃前20个)
---- | ---
![loss](https://github.com/wfnian/Kinect/blob/master/%E9%AA%A8%E9%AA%BC%E5%9D%90%E6%A0%87%E7%82%B9%E7%9A%84%E8%8E%B7%E5%8F%96%E5%85%A5%E5%BA%93/loss.png?raw=true)|![loss2](https://github.com/wfnian/Kinect/blob/master/%E9%AA%A8%E9%AA%BC%E5%9D%90%E6%A0%87%E7%82%B9%E7%9A%84%E8%8E%B7%E5%8F%96%E5%85%A5%E5%BA%93/loss2.png?raw=true)
### 1.3.1. 分类有问题啊。
![acc_loss](https://github.com/wfnian/Kinect/blob/master/loss_acc.png?raw=true)
### 1.3.2. 又出现奇迹了。
![玄学](https://github.com/wfnian/Kinect/blob/master/geez.png?raw=true)
### 1.3.3. 调整了学习率以及神经单元的层数
learning rate | number of iterations
---- | ---
![hh](https://github.com/wfnian/Kinect/blob/master/7-11-18-26.png?raw=true)|![hh](https://github.com/wfnian/Kinect/blob/master/7-11-18-30.png?raw=true)

## 1.4. 接下来几日日程安排
__时间__ | __任务__
---- | ---
__7/10__ 周二|__数据采集，考虑分类__
__7/11__ 周三|__分类__
__7/12__ 周四|__数据采集，增加抗干扰性__
__7/13__ 周五|__python 与c#交互，python负责识别，c#界面与数据的读取__
__7/14__ |__界面__
__7/15__ |__界面__
__7/16__ 周一|__文档，PPT__
__7/17-7/19__|__完善__
__7/20__|__答辩__
___
## 1.5. 注意事项


- 多线程调用很危险![attation](https://img.shields.io/badge/Attention-Serious-red.svg) 
- 注意文件的绝对路径
---
![url](https://qr.api.cli.im/qr?data=https%253A%252F%252Fgithub.com%252Fwfnian%252FKinect&level=H&transparent=false&bgcolor=%23ffffff&forecolor=%23000000&blockpixel=12&marginblock=1&logourl=http%3A&size=136&kid=cliim&key=6fc6080d5e7a26cb74bf361066319a3c)
