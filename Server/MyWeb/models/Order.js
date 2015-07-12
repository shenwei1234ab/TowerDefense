/**
 * Created by sw on 2015/6/9.
 */
var mongoose = require('mongoose');
var models = require('../routes/Schema');
var Order = models.Order;
var async=require("async");
//计划单
function OrderObj()
{

};
module.exports=OrderObj;
var PlaceObj=require('./Place');
var BaseInfoObj=require('./BaseInfo');

OrderObj.prototype.save = function save(order,callback)
{
    var modelOrder = models.Order;
    var orderEntity = new modelOrder(
        {
            groupid:order.groupid,
            orderid: new mongoose.Schema.ObjectId(),
            group:order.group,
            projects:order.projects,
            username:order.username,
            operator:order.operator,
            guide:order.guide,
            costs:order.costs,
            visitor:order.visitor,
            Actualcosts:order.Actualcosts,
            remark:order.remark
        }
    );
    orderEntity.orderid = orderEntity._id;
    //查找orderid对应的order
    modelOrder.count({groupid:order.groupid},function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            if(doc > 0)
            {
                //
                console.log("已经存在,无法添加");
                return callback(null,doc);
            }
            else
            {
                //添加
                orderEntity.save(function(error)
                {
                    if(error)
                    {
                        return callback(error);
                    }
                    else
                    {
                        return callback(null);
                    }
                });
            }

        }
    });
}





OrderObj.prototype.count=function count(field,callback)
{
    Order.count(field, function (error, count)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            return callback(null,count);
        }
    });
}


//查询指定的字段
OrderObj.prototype.get=function get(query,field,callback)
{
    Order.findOne(query,field,function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            if(!doc)
            {
                return callback('orderid 不存在');
            }
            return callback(null,doc);
        }
    });
};

//修改order表中某个字段的值
OrderObj.prototype.modify=function modify(arrQuery,strField,value,callback)
{
    this.get(arrQuery,{},function(error,doc)
    {
        if(error)
        {
            callback(error);
        }
        doc[strField]=value;
        doc.markModified(strField);
        doc.save(function(error)
        {
           if(error)
           {
               callback(error);
           }
            callback(null);
        });
    })
}


//获得下拉选项
OrderObj.prototype.getSelectValues = function (orderid,callback)
{
    //要返回的datas
    var datas =
    {
        years:null,
        months:null,
        days:null,
        //几个下拉列表的选项
        cities:null,
        traffic:null,
        hotels:null,
        //地点
        places:null,
        restaurants:null
    };
    //赋值
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

    var baseinfo=new BaseInfoObj();
    baseinfo.getAllCities(function(error,doc)
    {
        if(error)
        {
            callback(error,datas);
        }
        else
        {
            datas.cities = doc;
            baseinfo.getAlltraffic(function(error,doc)
            {
                if(error)
                {
                    callback(error,datas);
                }
                else
                {
                    datas.traffic = doc;
                    baseinfo.getAllHotels(function(error,doc)
                    {
                        if(error)
                        {
                            callback(error,datas);
                        }
                        else
                        {
                            datas.hotels = doc;
                            baseinfo.getAllPlaces(function(error,doc)
                            {
                                if(error)
                                {
                                    callback(error,datas);
                                }
                                else
                                {
                                    datas.places = doc;

                                    baseinfo.getAllRestaurants(function(error,doc)
                                    {
                                        if(error)
                                        {
                                            callback(error);
                                        }
                                        else
                                        {
                                            datas.restaurants=doc;
                                            callback(null,datas);
                                        }
                                    });
                                }
                            })
                        }
                    });
                }
            }) ;
        }
    });
}



