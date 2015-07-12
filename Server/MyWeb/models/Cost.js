var mongoose = require('mongoose');
var models = require('../routes/Schema');
var CostModel = models.Cost;



function CostObj()
{

};
module.exports = CostObj;

//更具season(busyseason?offseason?)获取
CostObj.prototype.queryCost = function queryCost(season,queryfield,callback)
{
    //var queryObj=new Object();
    //queryObj[season] = "1";
    CostModel.findOne({},{},function(error,doc)
    {
        if(error)
        {
            callback(error);
        }
        callback(null,doc[season][queryfield]);
    })
}


//获取
CostObj.prototype.queryAllCost=function queryAllCost(season,callback)
{
    CostModel.findOne({},{},function(error,doc)
    {
        if(error)
        {
            callback(error);
        }
        callback(null,doc[season]);
    })
}





