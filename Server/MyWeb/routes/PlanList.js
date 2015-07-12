

var mongoose = require('mongoose');

var models = require('./Schema');
var Order = models.Order;

exports.planList = function (req, res)
{
    Order.find({},{"_id":0,"guide":1, "orderid":1,"groupid":1}, function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
           console.log("doc is "+doc);
            console.log("doc._id is "+doc._id);
            res.render('PlanList', { datas:doc,username:req.query.username });
        }
    });

}


