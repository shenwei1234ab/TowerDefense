

var mongoose = require('mongoose');
var models = require('../routes/Schema');
var Log = models.Log;

//计划单
function LogObj()
{

};
module.exports=LogObj;
var PlaceObj=require('./Place');
var BaseInfoObj=require('./BaseInfo');

// insertNewRecord
LogObj.prototype.insertTest = function insertTest(orderid,user,guide,callback)
{
    //计算
    var modelOrder = models.Order;
    //因为不是纯净的jason数据，所以使用Entity方法
    var orderEntity = new modelOrder(
        {
            orderid:orderid,
            projects:new Array(),
            user:user,
            operator:null,
            guide:guide,
            costs:new Object(),
            visitor:null,
            Actualcosts:new Object()
        }
    );
    //查找orderid对应的order
    Order.count({orderid:orderid,user:user},function(error,doc)
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
};





