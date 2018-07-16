# Kinect应用-结合神经网络的姿势识别
_Kinect for detecting postures of chinese kongfu..._
## 1.1. README/wiki索引  

<!-- TOC -->

- [Kinect应用-结合神经网络的姿势识别](#kinect应用-结合神经网络的姿势识别)
    - [1.1. README/wiki索引](#11-readmewiki索引)
    - [1.2. 项目目录结构](#12-项目目录结构)
    - [1.3. 项目进度 ![Build Status](https://img.shields.io/badge/进度-Stagnation-lightgrey.svg)](#13-项目进度-build-statushttpsimgshieldsiobadge-stagnation-lightgreysvg)
    - [1.4. Tips](#14-tips)
    - [1.5. 基于神经网络的分类](#15-基于神经网络的分类)
        - [1.5.1. 调整学习率，层数，隐层单元数。](#151-调整学习率层数隐层单元数)
        - [1.5.2. 调整了学习率以及神经单元的层数](#152-调整了学习率以及神经单元的层数)
        - [1.5.3. 网络结构](#153-网络结构)
    - [1.6. 接下来几日日程安排](#16-接下来几日日程安排)
    - [1.7. 注意事项](#17-注意事项)

<!-- /TOC -->
## 1.2. 项目目录结构
- Kinect
    - neural_network  (神经网络分类))
        - backup
            - 23Classification.py
            - application.py
            - acc.png
            - loss.png
        - train_loss_acc_pic
            - acc.png
            - loss.png
        - 23Classification.py
        - application.py
        - train_data.txt
    - pic
        - pic.png
        - README配图
    - 骨骼坐标点获取入库 (C#获取Kinect SDK骨骼信息并调用Python model进行预测、界面)
        - C#...
    - README.md
    - 任务.txt
    - data.sql  (SQL SERVER 数据库)


## 1.3. 项目进度 ![Build Status](https://img.shields.io/badge/%E8%BF%9B%E5%BA%A6-Stagnation-lightgrey.svg)

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

## 1.4. Tips 
___
- 普通分类：获取相应关节点的坐标计算距离
- 交互式？静态？
- 多线程调用python，防止按钮以及整个窗体卡顿。
- 注意文件的读写占用冲突。解决方案：多线程交互式执行。 

- [Kinect 骨骼数据的提取]()  
- [基于kinect的人体动作识别系统，普通方法 （if-if-else-else）](https://blog.csdn.net/baolinq/article/details/78143748)  
- [pytorch手写数字识别，神经网络十分类问题](https://github.com/Elin24/learning_pyTorch_with_SherlockLiao/tree/master/Chapter_3)  
- [C#程序调用cmd执行命令](http://www.cnblogs.com/babycool/p/3570648.html)  
- [C#多线程编程](https://www.cnblogs.com/luxiaoxun/p/3280146.html) 
- [C#连接SQLServer数据库基本实现](https://www.cnblogs.com/wuqianling/p/5950667.html)
## 1.5. 基于神经网络的分类
### 1.5.1. 调整学习率，层数，隐层单元数。
![玄学](https://github.com/wfnian/Kinect/blob/master/pic/geez.png?raw=true)
### 1.5.2. 调整了学习率以及神经单元的层数
| loss                                                                                                  | acc                                                                                                  |
| ----------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- |
| ![](https://github.com/wfnian/Kinect/blob/master/neural_network/train_loss_acc_pic/loss.png?raw=true) | ![](https://github.com/wfnian/Kinect/blob/master/neural_network/train_loss_acc_pic/acc.png?raw=true) |
### 1.5.3. 网络结构
![](https://github.com/wfnian/Kinect/blob/master/pic/network_model.png?raw=true)
## 1.6. 接下来几日日程安排
| __时间__      | __任务__                                                |
| ------------- | ------------------------------------------------------- |
| __7/10__ 周二 | __数据采集，考虑如何分类__                              |
| __7/11__ 周三 | __分类__                                                |
| __7/12__ 周四 | __数据采集，增加抗干扰性__                              |
| __7/13__ 周五 | __python 与c#交互，python负责识别，c#界面与数据的读取__ |
| __7/14__      | __界面，数据再采集__                                    |
| __7/15__      | __界面，新数据处理__                                    |
| __7/16__ 周一 | __文档，PPT__                                           |
| __7/17-7/19__ | __完善__                                                |
| __7/20__      | __答辩__                                                |
___
## 1.7. 注意事项

- 多线程调用很危险 ![attation](https://img.shields.io/badge/Attention-Serious-red.svg) 
- 注意文件的绝对路径
---
项目二维码 

![url](https://qr.api.cli.im/qr?data=https%253A%252F%252Fgithub.com%252Fwfnian%252FKinect&level=H&transparent=false&bgcolor=%23ffffff&forecolor=%23000000&blockpixel=12&marginblock=1&logourl=http%3A&size=136&kid=cliim&key=6fc6080d5e7a26cb74bf361066319a3c)

**君不见高堂明镜悲白发，朝如青丝暮成雪。**[为什么？](http://www.kugou.com/share/52rRddct9V2.html?id=52rRddct9V2#hash=02EEC83F8075843B48E88792B999BE75)
