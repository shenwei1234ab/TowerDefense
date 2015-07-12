

var mongoose = require('mongoose');

var models = require('./Schema');
var Order = models.Order;

exports.planView = function (req, res)
{
    var id = req.query.orderid;
    console.log("id "+ id);
    Order.findOne({orderid:id}, {"_id":0, "guide":1, "orderid":1, "groupid":1, "projects":1}, function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {

            console.log("doc._id is " + doc);
            res.render('PlanView', { datas:doc, username:req.query.username });
        }
    });

}


