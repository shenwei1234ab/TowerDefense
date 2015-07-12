
var mongoose = require('mongoose');
var models = require('./Schema');

//获取封装的对象
var UserObj=require('../models/User');
var OrderObj=require('../models/Order');
var PlaceObj=require('../models/Place');
var async = require('async');


var orderObj=new OrderObj();

exports.CostSummary = function(req,res) {

    res.render('CostSummary');
}







exports.doCostSummary=function(req,res)
{
    var orderid=req.query.orderid;
    //要提交前台的数据
    var postDate=
    {
        //成本:cost
        cost:
        {
            placecost:
            {

            },
            mealcost:
            {

            }
        },
        //实际花费
        Actualcosts:
        {
            placecost:
            {
                /*costName:
                {
                    days: 1,
                    unitPrice: 0,
                    numbers: 0,
                    totalPrice: 0
                    //
                    cost:0
                    profit:0
                }*/
            },
            mealcost:
            {
                /*costName:
                 {
                 days: 1,
                 unitPrice: 0,
                 numbers: 0,
                 totalPrice: 0
                 }*/
            }
        }
    }
    async.auto({
        //查询orderid对应的游客人数
        queryVisitorCount:function (callback)
        {
            queryVisitorCount(orderid,callback);
        },
        //查询orderid对应的projects;
        queryProjects:function(callback)
        {
            queryProjects(orderid,callback);
        },
        //遍历projects获得所有的PlaceAndMeals
        getAllPlacesAndMeals:['queryProjects', function(callback,projects)
        {

            var projectArray = projects['queryProjects'].projects;
            getAllPlacesAndMeals(callback,projectArray);
        }],
        //获取成本
        queryCosts:function(callback)
        {
            queryCosts(orderid,callback);
        },
        //获取实际花费
        queryActualCosts:function(callback)
        {
            queryActualCosts(orderid,callback);
        },
        calculateCost:['queryVisitorCount','getAllPlacesAndMeals','queryCosts','queryActualCosts',function (callback,result) {
            var vistorCount=result["queryVisitorCount"];
            //成本表
            var costs=result["queryCosts"];



            //实际花费表
            var ActualCosts=result["queryActualCosts"];
            var places = result["getAllPlacesAndMeals"].places;
            var meals = result["getAllPlacesAndMeals"].meals;

            //遍历places
            places.forEach(function(place)
            {
                var costPlaceMap =costs.placecost;
                var  actualcostPlaceMap =ActualCosts.placecost;
                //填充成本
                //postDate.cost.placecost[place]=costPlaceMap[place];
                //填充实际成本
                //是否存在重复的
                if(postDate.Actualcosts.placecost.hasOwnProperty(place)) {
                    //去了两天
                    var key = postDate.Actualcosts.placecost[place];
                    key.days++;
                    key.totalPrice=key.days*key.unitPrice*key.vistorCount;
                    key.profit=calculateProfit(key.days,key.unitPrice,key.vistorCount,key.cost);
                }
                else
                {
                   var value=
                   {
                        days:1,
                        unitPrice: actualcostPlaceMap[place],
                        vistorCount: vistorCount,
                        totalPrice: 0,
                        cost:costPlaceMap[place],
                        profit:0
                   }
                    value.totalPrice =calculateTotalPrice(value.days,value.unitPrice,value.vistorCount);
                    value.profit=calculateProfit(value.days,value.unitPrice,value.vistorCount,value.cost);
                    postDate.Actualcosts.placecost[place]=value;
                }

            });

            //遍历meals
            meals.forEach(function(meal)
            {
                if(meal!='无')
                {

                    var mealMap =costs.mealcost;
                    var  actualcostMealMap =ActualCosts.mealcost;
                    //规范化meal
                    var key=meal;
                    //填充成本
                    // postDate.cost.mealcost[meal]=mealMap[key];
                    //填充实际成本
                    if(postDate.Actualcosts.mealcost.hasOwnProperty(meal)) {
                        var key = postDate.Actualcosts.mealcost[meal];
                        //去了两天
                        key.days++;
                        key.totalPrice=calculateTotalPrice(key.days,key.unitPrice,key.vistorCount);
                        key.profit=calculateProfit(key.days,key.unitPrice,key.vistorCount,key.cost);
                    }
                    else
                    {
                        var value=
                        {
                            days:1,
                            unitPrice: actualcostMealMap[key],
                            vistorCount: vistorCount,
                            totalPrice: 0,
                            cost:mealMap[meal],
                            profit:0
                        }
                        value.totalPrice =calculateTotalPrice(value.days,value.unitPrice,value.vistorCount);
                        value.profit=calculateProfit(value.days,value.unitPrice,value.vistorCount,value.cost);
                        postDate.Actualcosts.mealcost[meal]=value;
                    }
                }
            });
            callback(null,null);
        }]
    },function(err){
        if(err)
        {
            console.log(err);
        }
        else
        {
            console.log("postDate is "+JSON.stringify (postDate));
            //传递给前台
            res.render('doCostSummary', { postDate:postDate});
        }
    });
}

//计算总价
function  calculateTotalPrice(days,unitPrice,vistorCount)
{
    return days*unitPrice*vistorCount;
}


//计算利润
function calculateProfit(days,unitPrice,vistorCount,cost)
{
    return days*vistorCount*(unitPrice-cost);
}





function queryVisitorCount(orderid,callback)
{
    orderObj.get({orderid:orderid},{"visitor":1},function(error,doc)
    {
        if(error)
        {
            callback(error);
        }
        else
        {
            callback(null,doc.visitor.length);
        }
    })

}


function queryProjects(orderid,callback)
{
    //查询orderid对应的所有projects
    orderObj.get({orderid:orderid},{"projects":1},function(error,projects)
    {
        if(error)
        {
            callback(error);
        }
        else
        {
            //console.log("projects is " +projects);
            callback(null,projects);
        }
    });
}



//遍历projects获得所有的PlaceAndMeals
function getAllPlacesAndMeals(callback,projects)
{
    var placesArray = new Array();
    var mealsArray=new Array();

    //遍历projects获得所有的places
    projects.forEach(function(project)
    {
        placesArray = placesArray.concat(project.places);
    });
    //遍历projects获得所有的havemeals
    projects.forEach(function(project)
    {
        mealsArray = mealsArray.concat(project.meals);
    });
    var postData =
    {
        places:placesArray,
        meals:mealsArray
    }
    callback(null,postData);
}



function   queryCosts(orderid,callback)
{
    orderObj.get({orderid:orderid},{"costs":1},function(error,doc)
    {
        if(error)
        {
            callback(error);
        }
        else
        {
            callback(null,doc.costs);
        }
    })
}



function  queryActualCosts(orderid,callback)
{
    orderObj.get({orderid:orderid},{"Actualcosts":1},function(error,doc){
        if(error)
        {
            callback(error);
        }
        else
        {
            callback(null,doc.Actualcosts);
        }
    })
}


exports.ShowCost = function (req,res)
{
    var postDate=
    {
        group:null,
        groupid:null,
        visitorCount:0,
        placecost:null,
        mealcost:null,
        othercost:null
    }
    var orderid=req.query.orderid;
    orderObj.get({orderid:orderid},{},function(error,doc)
    {
        if(error)
        {
            console.log(error);
        }
        postDate.group=doc.group;
        postDate.groupid=doc.groupid;
        postDate.visitorCount=doc.visitor.length;
        if(doc.costs.hasOwnProperty("placecost"))
        {
            postDate.placecost = doc.costs.placecost;
        }
        if(doc.costs.hasOwnProperty("mealcost"))
        {
            postDate.mealcost = doc.costs.mealcost;
        }
        if(doc.costs.hasOwnProperty("othercost"))
        {
            postDate.othercost = doc.costs.othercost;
        }
        console.log("postDate is "+JSON.stringify(postDate));
        res.render('ShowCost', { postDate:postDate});
    })
}

/////////////////////////////////////////////////////////////////////////////////////


/*exports.doEditorCost=function(req,res)
{
    var orderid=req.query.orderid;
    getSummary(orderid,function(error,postDate)
    {
        if(error)
        {
            console.log(error);
        }
        else
        {
            res.render('doEditorCost', { postDate:postDate,orderid:orderid});
        }
    })
}*/


//生成合计
function getSummary(orderid,callback)
{
    //要提交的数据
    var postDate=
    {
        //结算价
         placecost:
         {

          /*   days:1,
             unitPrice: costPlaceMap[place],
             vistorCount: vistorCount,
             totalPrice: 0*/
         },
         mealcost:
         {

         },
        othercost:
        {

        }
    }
    async.auto({
        //查询otherCost
        queryotherCost:function (callback)
        {
            orderObj.get({orderid:orderid},{"costs":1,"_id":0},function(error,doc)
            {
                postDate.othercost = doc.costs.othercost;
                console.log( postDate.othercost );
                callback(null);
            })
        },

        //查询orderid对应的游客人数
        queryVisitorCount:function (callback)
        {
            queryVisitorCount(orderid,callback);
        },
        //查询orderid对应的projects;
        queryProjects:function(callback)
        {
            queryProjects(orderid,callback);
        },
        //遍历projects获得所有的PlaceAndMeals
        getAllPlacesAndMeals:['queryProjects', function(callback,projects)
        {

            var projectArray = projects['queryProjects'].projects;
            getAllPlacesAndMeals(callback,projectArray);
        }],
        //获取费用标准
        queryStandardCosts:function(callback)
        {
            queryCosts(orderid,callback);
        },
        calculateCost:['queryVisitorCount','getAllPlacesAndMeals','queryStandardCosts',function (callback,result) {
            var vistorCount=result["queryVisitorCount"];
            //费用标准
            var costs=result["queryStandardCosts"];
            var places = result["getAllPlacesAndMeals"].places;
            var meals = result["getAllPlacesAndMeals"].meals;

            //遍历places
            places.forEach(function(place)
            {
                //获取place标准
                var costPlaceMap =costs.placecost;
                //填充成本
                //是否存在重复的
                if(postDate.placecost.hasOwnProperty(place)) {
                    //去了两天
                    var value = postDate.placecost[place];
                    value.days++;
                    value.totalPrice=value.days*value.unitPrice*value.vistorCount;

                }
                else
                {
                    var value=
                    {
                        days:1,
                        unitPrice: costPlaceMap[place],
                        vistorCount: vistorCount,
                        totalPrice: 0
                    }
                    value.totalPrice =calculateTotalPrice(value.days,value.unitPrice,value.vistorCount);
                    postDate.placecost[place]=value;
                }

            });

            //遍历meals
            meals.forEach(function(meal)
            {
                if(meal!='无')
                {
                    var mealMap =costs.mealcost;
                    if(postDate.mealcost.hasOwnProperty(meal)) {
                        var value = postDate.mealcost[meal];
                        //去了两天
                        value.days++;
                        value.totalPrice=calculateTotalPrice(value.days,value.unitPrice,value.vistorCount);
                    }
                    else
                    {
                        var value=
                        {
                            days:1,
                            unitPrice: mealMap[meal],
                            vistorCount: vistorCount,
                            totalPrice: 0
                        }
                        value.totalPrice =calculateTotalPrice(value.days,value.unitPrice,value.vistorCount);
                        postDate.mealcost[meal]=value;
                    }
                }
            });
            callback(null,null);
        }]
    },function(err){
        if(err)
        {
            callback(err);
        }
        else
        {

            callback(null,postDate);
        }
    });
}

//生成成本单
exports.doEditorActualCost=function(req,res)
{
    var orderid=req.query.orderid;
    //要提交前台的数据
    var postDate=
    {
        //实际花费
        Actualcosts:
        {
            placecost:
            {
                /*costName:
                 {
                 days: 1,
                 unitPrice: 0,
                 numbers: 0,
                 totalPrice: 0
                 }*/
            },
            mealcost:
            {
                /*costName:
                 {
                 days: 1,
                 unitPrice: 0,
                 numbers: 0,
                 totalPrice: 0
                 }*/
            }
        }
    }
    async.auto({
        //查询orderid对应的游客人数
        queryVisitorCount:function (callback)
        {
            queryVisitorCount(orderid,callback);
        },
        //查询orderid对应的projects;
        queryProjects:function(callback)
        {
            queryProjects(orderid,callback);
        },
        //遍历projects获得所有的PlaceAndMeals
        getAllPlacesAndMeals:['queryProjects', function(callback,projects)
        {

            var projectArray = projects['queryProjects'].projects;
            getAllPlacesAndMeals(callback,projectArray);
        }],
        //获取实际花费
        queryActualCosts:function(callback)
        {
            queryActualCosts(orderid,callback);
        },
        calculateCost:['queryVisitorCount','getAllPlacesAndMeals','queryActualCosts',function (callback,result) {
            var vistorCount=result["queryVisitorCount"];
            //实际花费表
            var ActualCosts=result["queryActualCosts"];
            var places = result["getAllPlacesAndMeals"].places;
            var meals = result["getAllPlacesAndMeals"].meals;
            //遍历places
            places.forEach(function(place)
            {
                var  actualcostPlaceMap =ActualCosts.placecost;
                //填充实际成本
                //是否存在重复的
                if(postDate.Actualcosts.placecost.hasOwnProperty(place)) {
                    //去了两天
                    //var key = postDate.Actualcosts.placecost[place];
                    //key.days++;
                    //key.totalPrice=key.days*key.unitPrice*key.vistorCount;
                }
                else
                {
                    var value=actualcostPlaceMap[place];
                    /*{
                     //days:1,
                     unitPrice: actualcostPlaceMap[place],
                     vistorCount: vistorCount,
                     //totalPrice: 0
                     }*/
                    //value.totalPrice = value.days*value.unitPrice*value.vistorCount;
                    postDate.Actualcosts.placecost[place]=value;
                }

            });

            //遍历meals
            meals.forEach(function(meal)
            {
                if(meal !="无")
                {
                    var  actualcostMealMap =ActualCosts.mealcost;
                    //规范化meal
                    var key=meal;
                    //填充实际成本
                    if(postDate.Actualcosts.mealcost.hasOwnProperty(meal)) {
                        // var key = postDate.Actualcosts.mealcost[meal];
                        //去了两天
                        // key.days++;
                        //key.totalPrice=key.days*key.unitPrice*key.vistorCount;
                    }
                    else
                    {
                        var value=actualcostMealMap[key];
                        /*{
                         days:1,
                         unitPrice: actualcostMealMap[key],
                         vistorCount: vistorCount,
                         totalPrice: 0
                         }
                         value.totalPrice = value.days*value.unitPrice*value.vistorCount;*/
                        postDate.Actualcosts.mealcost[meal]=value;
                    }
                }
            });
            callback(null,null);
        }]
    },function(err){
        if(err)
        {
            console.log(err);
        }
        else
        {
            console.log("postDate is "+JSON.stringify (postDate));
            //传递给前台
            res.render('doEditorActualCost', { postDate:postDate,orderid:orderid});
        }
    });
}













































//修改Cost
/*function modifyCost(req,res,tableNameStr,urlStr)
{
    //处理前台的数据得到修改的数据
    var orderid=req.body.orderid;
    var placecost={};
    for(var i=0;i<req.body.placecostnumber;++i)
    {
        var key=req.body["placecost"+i];
        placecost[key]=req.body[key];
    }

    var mealcost={};
    for(var i=0;i<req.body.mealcostnumber;++i)
    {
        var key=req.body["mealcost"+i];
        mealcost[key]=req.body[key];
    }
    //转换成查询json
    var queryJson={};
    queryJson[tableNameStr]=1;
    async.series([
            //修改placecost
            function (callback)
            {
                orderObj.get({orderid:orderid},queryJson,function(error,doc)
                {
                    if(error)
                    {
                        callback(error);
                    }
                    ;
                    console.log("tableNameStr is "+tableNameStr);
                    console.log("doc is " +JSON.toString(doc[tableNameStr]));
                    for(var key in placecost  )
                    {
                        if(doc[tableNameStr].placecost.hasOwnProperty(key))
                        {
                            doc[tableNameStr].placecost[key] =  placecost[key];
                        }
                    }
                    //提交修改
                    doc.markModified(tableNameStr);
                    doc.save(function (error)
                    {
                        if(error)
                        {
                            callback(error);
                        }
                        else
                        {
                            callback(null,null);
                        }
                    })

                })
            },
            //修改mealcost
            function (callback)
            {
                orderObj.get({orderid:orderid},queryJson,function(error,doc)
                {
                    if(error)
                    {
                        callback(error);
                    }
                    ;
                    for(var key in mealcost)
                    {
                        //取出
                        var keyStr=key;

                        if(doc[tableNameStr].mealcost.hasOwnProperty(keyStr))
                        {
                            doc[tableNameStr].mealcost[keyStr] = mealcost[key];
                        }
                    }
                    //提交修改
                    doc.markModified(tableNameStr);
                    doc.save(function (error)
                    {
                        if(error)
                        {
                            callback(error);
                        }
                        else
                        {
                            callback(null,null);
                        }
                    })

                })
            }
        ],
        function(err, results)
        {
            if(err)
            {
                console.log(err);
            }
            else
            {
                res.redirect(urlStr);
            }
        });
}*/



exports.postEditorCost= function (req,res) {

    modifyCost(req,res,"costs","/EditorCost");
}





exports.EditorActualCost=function(req,res)
{
    res.render('EditorActualCost');
}






exports.postEditorActualCost=function(req,res)
{
    modifyCost(req,res,"Actualcosts","EditorActualCost");
}

