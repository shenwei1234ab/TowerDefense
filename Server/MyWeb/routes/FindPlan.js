

var mongoose = require('mongoose');

var models = require('./Schema');
var Order = models.Order;

exports.Find = function (req, res)
{
    res.render('FindPlan', { datas:null, username:req.query.username });
}

exports.FindPlan = function (req, res)
{
    var league = req.body.league;
    var groupid = req.body.groupid;
    var guide = req.body.guide;

    var quary = new Object();
    if (groupid != '')
        quary["groupid"] = groupid;
    if (guide != '')
        quary["guide"] = guide;

    console.log("quary " + quary);

    Order.find(quary, {"_id":0, "guide":1, "orderid":1, "groupid":1}, function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            console.log("Find doc is " + doc);
            res.render('FindPlan', { datas:doc, username:req.query.username });
        }
    });

}


