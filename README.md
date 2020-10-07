<b>(补充:另外一个不使用硬件设备使用OpenPose的姿态识别项目地址[posture_recognition](https://github.com/wfnian/posture_recognition))</b>

# Kinect应用-结合神经网络的姿势识别

<!-- TOC -->

- [Kinect应用-结合神经网络的姿势识别](#kinect应用-结合神经网络的姿势识别)
    - [项目背景](#项目背景)
    - [项目计划进度 ![Build Status](https://img.shields.io/badge/进度-Stagnation-lightgrey.svg)](#项目计划进度-build-statushttpsimgshieldsiobadge-stagnation-lightgreysvg)
    - [项目具体设计](#项目具体设计)
        - [1. 骨骼信息的获取。](#1-骨骼信息的获取)
        - [2. 骨骼信息存数据库。](#2-骨骼信息存数据库)
        - [3. 神经网络的设计](#3-神经网络的设计)
        - [4. 主程序界面设计](#4-主程序界面设计)
        - [5. Python与C#交流数据。](#5-python与c交流数据)
        - [6. 多线程处理](#6-多线程处理)
        - [7. 文件读写冲突处理](#7-文件读写冲突处理)
    - [项目展示与截图](#项目展示与截图)
        - [项目目录结构](#项目目录结构)
        - [界面及结果截图](#界面及结果截图)
    - [应用拓展领域](#应用拓展领域)
    - [参考文献与网址](#参考文献与网址)

<!-- /TOC -->

## 项目背景
> 1. 太极姿势识别必然涉及到分类问题，分类方法从简单的（*if-if-else-else*/也称为动物识别系统）到复杂的**svm**（*support vector machine*）再到新兴起的**深度学习分类**方法。各有优势。
> 2. Kinect主要在虚拟键盘映射的应用比较广泛，其次是骨骼信息，通过计算一些骨骼关节点的距离（二范数、曼哈顿距离），来作为高层信息输入到神经网络中去，免去了采集图像等卷积神经网络的复杂步骤。也能到达一个比较理想的结果。
> 3. 主要比较创新，利用了Kinect的骨骼信息，再结合现在很流行的神经网络以及**Pytorch**和TensorFlow等大型深度学习框架。
## 项目计划进度 ![Build Status](https://img.shields.io/badge/%E8%BF%9B%E5%BA%A6-Stagnation-lightgrey.svg)

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
## 项目具体设计
#### 1. 骨骼信息的获取。
>为了便于神经网络的训练，更为了提取高层信息，我们选取了差别比较明显十四组的两两关节点之间的曼哈顿距离（二范数）：
为了数据更加明显起见，我们为每一个都乘以权重**w=1000**，因此数据获取代码如下：
``` cs
hipcenter_handleft = 1000 * Math.Sqrt(
    Math.Pow((skeleton.Joints[JointType.HipCenter].Position.X - skeleton.Joints[JointType.HandLeft].Position.X), 2) +
    Math.Pow((skeleton.Joint[JointType.HipCenter].Position.Y - skeleton.Joints[JointType.HandLeft].Position.Y), 2) +
    Math.Pow((skeleton.Joints[JointType.HipCenter].Position.Z - skeleton.Joints[JointType.HandLeft].Position.Z), 2));
```
#### 2. 骨骼信息存数据库。
>考虑到数据量巨大，要频繁地增删查改，因此引入了数据库操作，因为是C#设计主界面从Kinect SDK获取信息，而SQL server与C#都是微软的产品，他们之间的连接相对比较容易，因此设计数据库的时候选择了SQL server。
C#连接数据库并对获取的数据执行SQL操作的代码核心如下：
``` cs
private String connsql = "server=.;database=bone_pos;integrated security=SSPI"; 
using (SqlConnection conn = new SqlConnection(connsql)){
            conn.Open();//打开数据库
            SqlCommand cmd = conn.CreateCommand();
            string SQLcmd = "insert into distance3 values(…)";
            cmd.CommandText = SQLcmd;
            cmd.ExecuteNonQuery();
}
```

#### 3. 神经网络的设计
>神经网络是用Pytorch实现的，因为是分类问题，我们受到了手写数字识别的程序的启发，手写数字识别是十分类，而太极姿势有二十四式，其中有两个是相同的姿势和名称，因此二十四分类问题变成了二十三分类。我们使用全连接神经网络，层数在最开始设计的是四层，输入数据是十四维度的，输出则自然是 *[0,22]*  ，神经网络的第一层隐藏层的单元数一共设计了四十个，第二层隐藏层单元数是五十个。
输入数据**x**的格式是二维数组。*x[0]* 的维度是 *14* 维，代表了所采集的 *14* 个长度。
输出 *y(target)* 的格式如下： 

*y = [0,1,2,3,...,22]*

>这里 *y* 是一维数组，与 *x* 的第一维度相对应，对应所属的姿势（分类结果）。
有了输入输出，定义的网络模型如下：
``` py
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
```
>损失函数使用的是交叉熵函数 **nn.CrossEntropyLoss**
采用随机梯度下降进行优化 *optim.SGD(model.parameters(), lr=1e-3)*，
其中设计的 *learning rate = 0.001*，即 *1e-3*.
然后进行 *3000* 次左右的迭代便是：
``` py
for epoch in range(3000):
    x_data = Variable(torch.tensor(x))
    target = Variable(torch.tensor(y))
    #=================forward 正向传播 ==================
    out = model(x_data)
    loss = criterion(out, target)
    #================backward 反向传播 ==================
    optimizer.zero_grad()
    loss.backward()
    optimizer.step()
    # if epoch %1 ==0:
    plt_loss.append(loss.item())
    #===============accurate 计算准确率 ==================
    vect = out.detach().numpy().tolist()
    acc = 0
    for i in range(len(vect)):
        if y[i] == vect[i].index(max(vect[i])):
            acc = acc+1
    acc = acc/len(vect)
    acc_.append(acc)
```
>最后将模型保存起来便是。*torch.save(model,’model.pth’)*,方便下次进行预测时直接调用模型预测。加载的时候 *model = torch.load('model.pth')* 就可以直接利用已经训练好的模型，进行正向传播预测结果。
预测结果是和 *y*同等 *shape*的数组，其中最大值所在的下表索引便是我们要结果值。
最后训练的网络模型 *loss*和 *acc*如下：

| loss                                                                                                  | acc                                                                                                  |
| ----------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- |
| ![](https://github.com/wfnian/Kinect/blob/master/neural_network/train_loss_acc_pic/loss.png?raw=true) | ![](https://github.com/wfnian/Kinect/blob/master/neural_network/train_loss_acc_pic/acc.png?raw=true) |
#### 4. 主程序界面设计
>主界面是用C#设计的，其中我们继承了来自CSkin的CCSkinMain这个窗体库，使得窗体不是那么呆板，窗体分为四个基本区域，左上、右上、左下、右下，左上显示的是实时捕获到的骨骼。右上展示的是太极姿势集合图。左下分别有两幅图，是网络的训练loss图与数据正确率acc的图，右下左边的layoutPanel里有12个label用来显示实时距离参数，每隔5秒重新计算一下。右边的训练网络是指加载网络模型，是采用多线程加载的，由于加载比较耗时，若果不采用多线程，则会导致窗体卡顿甚至奔溃，其加载完的loss函数图以及正确率acc的图都在左下显示，同时也有stressProcessBar显示进度状态。点击识别按钮便是核心，连接python与C#，将识别结果显示在下方，当设备未连接时，该按钮不可用。最下方状态栏显示启动时间，Kinect连接信息，processBar显示神经网络训练进度。
#### 5. Python与C#交流数据。
>因为姿势预测需要C#获取按钮事件时的实时数据，传递给Python已经训练好的模型中去，而C#又要获得Python 模型进行预测的结果，因此就涉及到了语言间的相互调用的问题。
- 第一种解决方案是NuGet包管理器的IronPython，但是当我们安装了IronPython后发现环境是和Windows的python环境不同，那么就有torch库的不存在的问题。  
- 第二种解决方案是命令行方式，利用python的sys库的sys.argv获得传入的参数，并将结果print出来，在对应的C#里面也有相应的处理流程.
#### 6. 多线程处理
>在执行训练网络的时候，我们采用了多线程若果不采用多线程，会因为网络训练过程较长而导致窗体卡顿甚至黑屏，则必须要采用多线程。
在显示实时数据时候，通过等待异步，我们就不会总是持有主线程的控制，这样就可以在不发生跨线程调用异常的情况下完成多线程对winform多线程控件的控制了。
``` cs
private delegate void FlushClient();

FlushClient fc = new FlushClient(ShowIn12Label);//多线程 ShowIn12Label 主函数
fc.BeginInvoke(null, null);
```
>这样便开启了等待异步，从而可以不发生跨线程调度异常。
#### 7. 文件读写冲突处理
>显示图片的时候，窗体程序是读文件，而训练网络生成要显示图片是文件的写操作，读写冲突，解决方法是交叉执行。起初的时候我们用了一个文件，发现点击训练的时候图片一直不变，后来通过上网查资料，调试执行发现了是文件读写冲突占用的问题，交叉执行通俗来讲就是将训练网络源文件复制一份，分开了执行，在显示的时候交叉调用就可以。


## 项目展示与截图

#### 项目目录结构
- Kinect
    - neural_network  (神经网络分类)
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
#### 界面及结果截图

>下图所示是正在训练网络的截图，进度条在显示进度。迭代次数为 *3000* 次，使用的是CPU训练，因此比较耗时。
![](https://github.com/wfnian/Kinect/blob/master/pic/res1.png?raw=true=200*300)
下图是当测试同学摆出“**海底针**”姿势的时候点击“点击识别”按钮后识别的结果图，结果与预期结果一致。
![](https://github.com/wfnian/Kinect/blob/master/pic/res2.png?raw=true)
## 应用拓展领域
> 太极姿势识别已经做到了比较理想的效果，并且太极作为中国民间一项被大众所喜闻乐见的体育运动，我们的项目可以有很大的用途，用来识别姿势属性。此外，我们可以结合 *RGBD* 深度摄像头与神经网络做许多的事情，例如手势识别，行人检测，人体行为分析，互动教学，目标跟踪，图像语义分割等方面的应用。


## 参考文献与网址  
-  [基于kinect的人体动作识别系统，普通方法 （if-if-else-else）](https://blog.csdn.net/baolinq/article/details/78143748)
-  [pytorch手写数字识别，神经网络十分类问题](https:\github.com\Elin24\learning_pyTorch_with_SherlockLiao\tree\master\Chapter_3)
- [C#程序调用cmd执行命令](http://www.cnblogs.com/babycool/p/3570648.html)
- [C#多线程编程](https://www.cnblogs.com/luxiaoxun/p/3280146.html)
- [C#连接SQLServer数据库基本实现](https://www.cnblogs.com/wuqianling/p/5950667.html)
- [PyTorch Tutorials](https://pytorch.org/tutorials/)
- [PyTorch: Softmax多分类实战 - CSDN博客](https://www.baidu.com/link?url=lTWjLLpvGx_4DJveK3BxTepk3jn3gihsHq6iujMsDdMfShI1Ofdk6hTg3qNqGFbCmwkHW9nFUyrmslY-396Jew68xLjqOoU3jtEJzuJ3_Ei&wd=&eqid=dadd5ad10003b099000000035b4e8d49)   
- [C#编程指南](https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/index)
- [Kinect简介](https://wenku.baidu.com/view/1d309d14168884868762d692.html?from=search)
- [Kinect体感交互技术](https://wenku.baidu.com/view/9a2a02118e9951e79b8927c5.html?from=search)
- 深度学习入门之PyTorch 廖星宇 著   
- 《机器学习》 周志华 著   
- 《统计学习方法》 李航 著   
- 吴恩达深度学习 COURSERA   
