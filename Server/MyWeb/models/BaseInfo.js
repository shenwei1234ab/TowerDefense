
var mongoose = require('mongoose');
var models = require('../routes/Schema');
var Baseinfo = models.Baseinfo;
var costModel=models.Cost;

//计划单
function BaseInfoObj()
{

};
module.exports=BaseInfoObj;


BaseInfoObj.prototype.getAllGuides = function getAllGuides(callback)
{
    Baseinfo.findOne({},{"guides":1},function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            return callback(null,doc.guides);
        }
    });
};

BaseInfoObj.prototype.getAllHotels = function getAllHotels(callback)
{
    Baseinfo.findOne({},{"hotels":1},function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            return callback(null,doc.hotels);
        }
    });
};

BaseInfoObj.prototype.getAllGroups = function getAllGroups(callback)
{
    Baseinfo.findOne({},{"groups":1},function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            return callback(null,doc.groups);
        }
    });
};


BaseInfoObj.prototype.getAllCities = function getAllCities(callback)
{
    Baseinfo.findOne({},{"cities":1},function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            return callback(null,doc.cities);
        }
    });
};



BaseInfoObj.prototype.getAlltraffic = function getAlltraffic(callback)
{
    Baseinfo.findOne({},{"traffic":1},function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            return callback(null,doc.traffic);
        }
    });
};


BaseInfoObj.prototype.getAlltraffic = function getAlltraffic(callback)
{
    Baseinfo.findOne({},{"groups":1},function(error,doc)
    {
        if(error)
        {
            return callback(error);
        }
        else
        {
            return callback(null,doc.groups);
        }
    });
}





BaseInfoObj.prototype.getAllRestaurants = function getAllRestaurants(callback)
{
    costModel.findOne({},{},function(error,doc)
    {
        if(error)
        {
            callback(error);
        }
        var keys=
        {

        }
        var restaurants=new Array();
        var restaurantArray=doc.busyseason.mealcost;

        for(var property in restaurantArray)
        {
            var key=property.split('/')[0];
            if(keys.hasOwnProperty(key))
            {
                continue;
            }
            else
            {
                keys[key]=0;
                restaurants.push(key);
            }
        }
        callback(null,restaurants);
    })
}


BaseInfoObj.prototype.getAllPlaces = function getAllPlaces(callback) {
    costModel.findOne({}, {}, function (error, doc) {
        if (error) {
            callback(error);
        }
        var places = new Array();
        var placecostArray = doc.busyseason.placecost;

        for (var property in placecostArray) {
            places.push(property);
        }
        callback(null, places);
    })
}


