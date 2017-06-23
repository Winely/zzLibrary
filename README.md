# 图书馆管理系统
> 1452764 何冬怡
> 1452769 张鸿羽

本项目为一个图书馆馆藏管理系统。

## 项目功能
- 管理员和用户两种身份
- 用户/管理员查询书籍
- 管理员借还书
- 管理员管理用户信息
- 管理员录入/删除馆内书籍/复本
- 管理员查询借阅用户记录
- 用户查询借阅信息

## 项目结构
- 前后端分离开发
- 前端： **Vue.js**框架
- 后端： **ASP.NET Web API 2.0**
- ORM： Entity Framework 6
- 数据库：MySQL
- COM组件：MD5加密模块
- C++ dll：罚款计算功能
- 多线程：使用 async/await 进行数据库读取

## 项目源码地址
- 后端：[https://github.com/Winely/zzLibrary](https://github.com/Winely/zzLibrary)
- 前端：[https://github.com/PNIDEMOOO/zzlibrary](https://github.com/PNIDEMOOO/zzlibrary)

## 开发工具
前端使用 WebStorm 2017.2 开发，后端使用 Visual Studio 2017 开发，Web API通过 Postman 进行调试。

## 代码目录
### /zzLibrary
ASP.NET的项目本体。
### /ZZLibModel
为项目的ORM Model类封装。
### /ZZLibDAO
为项目的数据库处理类封装。
### /Encryptor
为COM组件，提供MD5加密功能，本项目用以加密用户名密码。
### /Penalty
C++ dll库，提供罚款的计算模块。

## 安装说明
- 开启IIS 8功能
- 双击`zz-library.exe`完成安装
