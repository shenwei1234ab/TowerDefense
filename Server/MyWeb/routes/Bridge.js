//连接两个页面


var mongoose = require('mongoose');
//�����Զ���İ�
var models = require('./Schema');
var OrderObj=require('../models/Order');

/*
exports.NewOrderEditorNext = function(req,res)
{
    res.redirect("/Remarks?orderid=" + req.body.orderid);
}
*/

var orderObj=new OrderObj();

//连接EditorOrder和EditorProject
exports.EditorDailyPlan = function(req,res)
{
    //发送给前台页面
    var postDate=
    {
        orderid:req.body.orderid,
        seletedprojectsIndex:req.body.postprojectsindex,
        selectedyear:null,
        selectedmonth:null,
        selectedday:null,
        seletedcity:null,
        seletedtraffic:req.body.traffic,
        seletedplaces:req.body.place,
        seletedhotel:req.body.hotel,
        seletedmeals:req.body.meals
    }


    orderObj.get({orderid:postDate.orderid},{"projects":1},function(error,doc)
    {
        if(error)
        {
            console.log(error);
        }
        else
        {
            //获得已选择的值
            var seleteddate=doc.projects[postDate.seletedprojectsIndex].date;
            postDate.selectedyear =seleteddate.getFullYear();
            postDate.selectedmonth=seleteddate.getMonth()+1;
            postDate.selectedday=seleteddate.getDate();
            postDate.seletedcity=doc.projects[postDate.seletedprojectsIndex].city;
            postDate.seletedtraffic=doc.projects[postDate.seletedprojectsIndex].traffic;
            postDate.seletedplaces=doc.projects[postDate.seletedprojectsIndex].places;
            postDate.seletedhotel=doc.projects[postDate.seletedprojectsIndex].hotel;
            postDate.seletedmeals=doc.projects[postDate.seletedprojectsIndex].meals;
            //获得下拉选项
            orderObj.getSelectValues(req.body.orderid,function(error,datas)
            {
                if(error)
                {
                    console.log(error);
                }
                else
                {
                    postDate.years = datas.years;
                    postDate.months=datas.months;
                    postDate.days=datas.days;
                    postDate.cities=datas.cities;
                    postDate.traffic=datas.traffic;
                    postDate.hotels=datas.hotels;
                    postDate.places=datas.places;
                    postDate.restaurants = datas.restaurants;
                    console.log("提交前台的数据是"+JSON.stringify(postDate));
                    res.render('EditorProject', { postDate:postDate });
               }
            })
        }
    })
}


exports.doEditorDailyPlan = function(req,res)
{
    console.log("提交修改");
    var orderid = req.body.orderid;
    //project所在index
    var postprojectsindex=req.body.postprojectsindex;
    var project=
    {
        date:new Date(req.body.month+'/'+req.body.day+'/'+req.body.year),
        city:req.body.city,
        traffic:req.body.traffic,
        places:new Array(),
        hotel:req.body.hotel,
        meals:req.body.restaurants+req.body.meals
    };
    /////////////////////////////////////////规范化前台的select内容为Array
    //规范化Selectedplaces
    
    var type = typeof (req.body.Selectedplaces);
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

    /*type = typeof(req.body.meals);
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

    orderObj.get({orderid:orderid},{},function(error,doc)
    {
        if(error)
        {
            console.log(error);
        }
        else
        {
            //修改
            doc.projects[postprojectsindex]=project;
            doc.markModified('projects');
            doc.save(function(error)
            {
                if(error)
                {
                    console.log(error);
                }
                else
                {
                    res.redirect("/EditorOrder");
                }
            })
        }
    })
}
