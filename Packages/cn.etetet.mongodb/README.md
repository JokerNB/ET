# 数据库补丁工具的作用
1. 团队开发，一个开发人员开发好友系统，需要给数据库添加一条全局数据，或者修改已有数据。其他人更新版本就缺少这些变化
2. 线上项目，停服维护，增加了新功能，也有向数据库添加数据，或者修改数据库中的数据的需求
3. 正确的做法是数据库保存了一个版本号，开发人员提供数据库补丁，服务器在开发模式需要检测版本号，如果版本号低，则自动执行补丁到最新的版本

# 数据库补丁工具的开发：开发者假如有修改数据库的需求，那么他需要修改两个文件
1. 修改DBPatcher_CreateDB，这个文件是用来创建数据库的，新的使用者运行CreateDB会调用这个代码来创建新的数据库
2. 增加DBPatcher，文件命名DBPatcher_001 001是补丁编号，类上面DBPatcherAttribute也是补丁编号，老的使用者会执行补丁到最新补丁

# 数据库补丁工具的使用
1. 在ET目录建立一个MongoDB目录，里面放置MongoDB数据库程序，注意mongod.exe就在MongoDB目录中，不要有子目录。
2. 在MongoDB目录建立一个data目录，用来存放数据
3. 点击Unity菜单ET->MongoDB->StartMongo，这样就启动了Mongo数据库
4. 启动MongoDB补丁工具，启动命令是  dotnet .\Bin\ET.App.dll --SceneName=MongoDB --Console=1  注意SceneName是MongoDB
5. 启动之后，可以输入CreateDB DropDB UpdateDB三个命令，分别是创建数据库，删除数据库，更新数据库
6. CreateDB创建数据库是执行DBPatcher_CreateDB代码，进行数据库创建，插入数据，建立索引都可以在这里操作
7. DropDB删除数据库是删除所有库
8. UpdateDB更新数据库，是将当前数据库数据依次执行补丁到最新版本