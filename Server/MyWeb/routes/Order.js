
var mongoose = require('mongoose');
var models = require('./Schema');

//获取封装的对象
var UserObj=require('../models/User');
var OrderObj=require('../models/Order');
var PlaceObj=require('../models/Place');
var BaseInfoObj=require('../models/BaseInfo');
var MealObj=require('../models/Meals');
var OtherObj=require('../models/Other');
var CacheObj=require('../models/Cache');
var ToolObj=require('../models/Tool');
var CostObj=require('../models/Cost');
mongoose.connect('mongodb://localhost/mldndb');


var async = require('async');
var orderobj = new OrderObj();

var otherObj=new OtherObj();
var cacheObj=new CacheObj();
var baseinfoObj=new BaseInfoObj();
var mealObj=new MealObj();
var toolObj=new ToolObj();
var costObj=new CostObj();
exports.NewOrder = function(req,res)
{
    var username=req.query.username;
    //要传递给前台的所有变量
    var datas =
    {
        guides:null,
        years:null,
        months:null,
        days:null,
        groups:null,
        cities:null,
        //traffic:null,
        hotels:null,
        //地点
        places:null,
        restaurants:null,
        //已经输入好的条目
        selectedprojects:new Array(),
        selectedvalue:null
    };
    async.parallel([
        function setDate(callback)
        {
            datas.years=new Array();
            for(var i=0,j=2015;j<2020;++i,++j)
            {
                datas.years[i] = j;
            }
            datas.months=new Array();
            for(var i=0;i<=11;++i)
            {
                datas.months[i] = i+1;
            }
            datas.days=new Array();
            for(var i=0;i<=30;++i)
            {
                datas.days[i] = i+1;
            }
            callback(null,null)
        },
        function getAllGuides(callback)
        {
            baseinfoObj.getAllGuides(function(error,guides)
            {
                if(error)
                {
                    callback(error);
                }
                datas.guides=guides;
                callback(null,guides);
            })
        },
        function getAllGroups(callback)
        {
            baseinfoObj.getAllGroups(function(error,groups)
            {
                if(error)
                {
                    callback(error);
                }
                datas.groups=groups;
                callback(null,groups);
            })
        },
        function getAllCities(callback)
        {
            baseinfoObj.getAllCities(function(error,cities)
            {
                if(error)
                {
                    callback(error);
                }
                datas.cities=cities;
                callback(null,cities);
            })
        },
        function getAllHotels(callback)
        {
            baseinfoObj.getAllHotels(function(error,hotels)
            {
                if(error)
                {
                    callback(error);
                }
                datas.hotels=hotels;
                callback(null,hotels);
            })
        },
        function getAllPlaces(callback)
        {
            baseinfoObj.getAllPlaces(function(error,places)
            {
                if(error)
                {
                    callback(error);
                }
                datas.places=places;
                callback(null,places);
            })
        },
        function getAllRestaurants(callback)
        {
            baseinfoObj.getAllRestaurants(function(error,restaurants)
            {
                if(error)
                {
                    callback(error);
                }
                datas.restaurants=restaurants;
                callback(null,restaurants);
            })
        }
    ],function(error,results)
    {
        //如果是不是第一次提交
        datas.selectedprojects = cacheObj.getDate(username+"selectedproject");
        datas.selectedvalue = cacheObj.getDate(username+"selectedvalue");
       console.log("提交的数据projects"+datas);
        res.render('NewOrder', {datas: datas, username: username});
    });
}




exports.AddDailyPlan = function(req, res) {
    var username=req.body.username;
    var selectedvalue =
    {
      selectedgroupid:req.body.groupid,
      selectedgroup:req.body.groups,
      selectedseason:req.body.season,
      selectedguide:req.body.guide
    }
    var project=
    {
        date:new Date(req.body.month+'/'+req.body.day+'/'+req.body.year),
        city:req.body.city,
        traffic:req.body.traffic,
        places:new Array(),
        hotel:req.body.hotel,
        meals:new Array()
    };
    //////////////////////////////////////////////////////////////战且meals只有一个
    var meal=null;
    if(req.body.meal =="无")
    {
        meal= req.body.meal;
    }
    else
    {
        meal=req.body.restaurant+"/"+req.body.meal;
    }
    project.meals.push(meal);
    /////////////////////////////////////////规范化前台的select内容为Array
    var type = typeof(req.body.Selectedplaces);
    if(type !=  'undefined')
    {
        if(type == 'string')
        {
            project.places.push(req.body.Selectedplaces);
        }
        else
        {
            project.places=req.body.Selectedplaces;
        }
    }
   /* type = typeof(req.body.meals);
    if(type !=  'undefined')
    {
        if(type == 'string')
        {
            project.meals.push(req.body.restaurant+req.body.meals);
        }
        else
        {
            //选择了两个
            //project.meals=req.body.restaurants+"/"+req.body.meals;
        }
    }*/

    //toolObj.ConvertToArray(req.body.Selectedplaces,project.places);

    //保存数据
    cacheObj.saveDate(username+"selectedproject",project,false);
    cacheObj.saveDate(username+"selectedvalue",selectedvalue,true);
    res.redirect("/NewOrder?username="+username);
}


exports.SavePlan = function SavePlan(req,res)
{
    var groupid=req.body.groupid;
    var season=req.body.season;
    //标准化
    if(season == "淡季")
    {
        season = "offseason";
    }
    else if(season == "汪季")
    {
        season = "busyseason";
    }
    else
    {
        console.log("season 不合法"+season);
        return;
    }
    if(groupid == null)
    {
        console.log("orderid is null");
        return ;
    }
    //判断groupid是否存在
    orderobj.count({groupid:groupid},function(error,num)
    {
        if(error)
        {
            console.log(error);
            return ;
        }
        if(num >= 1)
        {
            console.log("groupid is exist");
            return ;
        }
        var username=req.body.username;
        var postOrder=
        {
            groupid:groupid,
            orderid:0,
            group:req.body.group,
            projects:cacheObj.getDate(username+"selectedproject"),
            username:username,
            operator:null,
            guide:req.body.guide,
            costs:new Object(),
            visitor:new Array(),
            Actualcosts:null,
            remark:req.body.remark
        }
        cacheObj.clearDate(username+"selectedproject");
        cacheObj.clearDate(username+"selectedvalue");
        //获取游客人数
        postOrder.visitor = req.body.visitor.split('/');
        if(postOrder.visitor == "")
        {
            console.log("input visitor is null");
            return ;
        }
        //获取游客人数
        var visitorCount=postOrder.visitor.length;
        if(visitorCount<=0)
        {
            console.log("visitor count is <=0");
            return ;
        }

        //要保存的数据
        postOrder.costs=
        {
            //结算价
            placecost:
            {

                /*   days:1,
                 unitPrice: costPlaceMap[place],
                 vistorCount: vistorCount,
                 totalPrice: 0*/
            },
            mealcost:new Object(),
            othercost:new Object()
        }
        //确定cost标准
       async.auto({
            //读取place表获取places标准并且转化成jason
            loadCost: function (callback)
            {
                //更具Season获取Cost标准
                costObj.queryAllCost(season,function(error,Cost) {
                    if(error)
                    {
                        callback(error);
                    }
                    console.log("Cost标准是"+Cost);
                    callback(null,Cost);
                });
            },
            //保存计划单
            saveOrders:['loadCost',function(callback,result){
                //费用标准
                //var placecost=result["loadCost"].placecost;
               // var mealcost=result["loadCost"].mealcost;
               // var othercost=result["loadCost"].othercost;
                var places=null;
                var meals=null;
                postOrder.costs.othercost= result["loadCost"].othercost;

                //遍历projects获得所有的PlaceAndMeals
                getAllPlacesAndMeals(postOrder.projects,function(postData)
                {
                    places=postData.places;
                    meals=postData.meals;
                });
                //遍历places
                places.forEach(function(place)
                {
                    //获取place标准
                    var costPlaceMap =result["loadCost"].placecost;

                    //是否存在重复的
                    if( postOrder.costs.placecost.hasOwnProperty(place)) {
                        //去了两天
                        var value = postOrder.costs.placecost[place];
                        value.days++;
                    }
                    else
                    {
                        var value=
                        {
                            days:1,
                            unitPrice: costPlaceMap[place],
                            vistorCount: visitorCount
                        }
                        postOrder.costs.placecost[place]=value;
                    }
                });
                //遍历meals
                meals.forEach(function(meal)
                {
                    if(meal!='无')
                    {
                        var mealMap =result["loadCost"].mealcost;
                        if(postOrder.costs.mealcost.hasOwnProperty(meal)) {
                            var value = postOrder.costs.mealcost[meal];
                            //去了两天
                            value.days++;
                        }
                        else
                        {
                            var value=
                            {
                                days:1,
                                unitPrice: mealMap[meal],
                                vistorCount: visitorCount
                            }
                            postOrder.costs.mealcost[meal]=value;
                        }
                    }
                })
                postOrder.Actualcosts = postOrder.costs;
                //保存计划单
               orderobj.save(postOrder,function(error)
               {
                   if(error)
                   {
                       callback(error)
                   }
                   callback(null,null);
               })
            }]
        },function(err,result)
        {
            if(err)
            {
                console.log(err);
            }
            res.redirect("/PlanList?username=" + username);
        });
    });
};




exports.EditorOrderNext=function EditorOrderNext(req,res)
{
    res.render('EditorOrder');
}

//遍历projects获得所有的PlaceAndMeals
function getAllPlacesAndMeals(projects,callback)
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
    callback(postData);
}



exports.EditorOrder = function EditorOrder(req,res)
{
    //要提交前台的数据
    var postguides=null;
    //查询guide下拉列表的选项
    baseinfoObj.getAllGuides(function(error,guides)
    {
         if(error)
         {
             console.log(error);
             return;
         }
        else
         {
             postguides = guides;
             orderobj.get({orderid:req.query.orderid},{},function(error,doc)
             {
                 if(error)
                 {
                     console.log(error);
                 }
                 else
                 {
                     if(!doc)
                     {
                         console.log("Orderid不存在");
                        return;
                     }
                     else
                     {
                         //传递给前台
                         res.render('EditorOrder', { doc:doc,guides:postguides});
                     }
                 }
             })
         }
    })
}


//修改导游和游客的信息
exports.postEditorOrder = function (req,res)
{

    for(var value in req.body.placecost)
    {
        console.log(value);
    }

    if(typeof(req.body.orderid ) == "undefined")
    {
        console.log("orderid ) == undefined");
        return;
    }

   var visitor = req.body.visitor;
   var orderid = req.body.orderid;
    var guide=req.body.guide;
   if(! visitor instanceof (Array))
   {
       console.log("visitor is not a Array");
       return ;
   }
   //获得新的placecost
    var postPlaceCost=new Object();
    //获得新的mealcost
    var postMealCost=new Object();


    //获得所有的key值
    var placecostKey=new Array();
    var mealcostKey=new Array();
    for(var index=0;index<req.body.placeindex;++index)
    {
        placecostKey.push(req.body["placecost"+index]);
    }


    for(var index=0;index<req.body.mealindex;++index)
    {
        mealcostKey.push(req.body["mealcost"+index]);
    }

    placecostKey.forEach(function(key)
    {
        postPlaceCost[key]=
        {
            vistorCount: req.body[key+"vistorCount"],
            unitPrice:req.body[key+"unitPrice"],
            days:req.body[key+"days"]
        }
    })


    mealcostKey.forEach(function(key)
    {
        postMealCost[key]=
        {
            vistorCount: req.body[key+"vistorCount"],
            unitPrice:req.body[key+"unitPrice"],
            days:req.body[key+"days"]
        }
    })

/*
   orderobj.modify({orderid:orderid},"guide",req.body.guide ,function(error,doc)
       {
            if(error)
            {
                console.log(error);
                return;
            }
            else
            {
                orderobj.modify({orderid:req.body.orderid},"visitor",req.body.visitor ,function(error,doc) {
                    if(error)
                    {
                        console.log(error);
                        return ;
                    }
                    res.redirect("/EditorOrder?orderid="+req.body.orderid);
                })
            }
       }


   )*/
    orderobj.get({orderid:orderid},{},function(error,doc)
    {
        if(error)
        {
            console.log(error);
            return;
        }
        doc.guide=req.body.guide;
        doc.orderid=req.body.orderid;
        doc.visitor=req.body.visitor;
        doc.costs.placecost=postPlaceCost;
        doc.costs.mealcost=postMealCost;
        doc.markModified("costs");
        doc.save(function(error)
        {
            if(error)
            {
                console.log(error);
            }
            res.redirect("/EditorOrder?orderid="+req.body.orderid);
        });

    })
}



/*


exports.doEditorOrderPostguide = function doEditorOrderPostguide(req,res)
{
    //更新
    var orderobj = new OrderObj();
    console.log("req.body.guide+"+req.body.guide);
    orderobj.updateGuide(req.body.orderid,req.body.guide,function(error,doc)
    {
        if(error)
        {
            console.log(error);
        }
        else
        {
            res.redirect("/doEditorOrder?orderid="+req.body.orderid);
        }
    });
}


exports.doEditorOrderPostvisitor = function doEditorOrderPostvisitor(req,res)
{
     var visitorArray=req.body.visitor.split('/');
    var orderobj = new OrderObj();
    orderobj.updatevisitor(req.body.orderid,visitorArray,function(error,doc)
    {
        if(error)
        {
            console.log(error);
        }
        else
        {
            res.redirect("/doEditorOrder?orderid="+req.body.orderid);
        }
    });
}
*/










