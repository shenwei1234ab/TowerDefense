
/*
 * GET users listing.
 */

var mongoose = require('mongoose');	//
var Schema = mongoose.Schema;	//


//定义Schema(不能操作数据库)
//录入员
var UserSchema = new Schema(
{
    //userid: String,
    name: String,
    password: String,
    type:String
});
//计调员
var OpertorSchema = new Schema(
    {
        userid: String,
        name: String,
        password: String
    }
);
/*//景点费用
var PlaceSchema = new Schema(
    {
        placename:String,
        cost:Number
    }
);

//接待费用
var MealsSchema = new Schema(
    {
        //级别(人数)
        visitorcount:Number,
        comprehensivecost:Number,
        lunch:Number,
        dinner:Number,
        total:Number
    }
);
//其他费用
var OthersSchema=new Schema(
 {
 costname:String,
 cost:Number
 }
 );*/


var CostSchema =new Schema(
    {
        busyseason:
        {
            placecost:Object,
            mealcost:Object,
            othercost:Object
        },
        offseason:
        {
            placecost:Object,
            mealcost:Object,
            othercost:Object
        }

    }
)


//计划单
var OrderSchema = new Schema(
    {
        groupid:String,
        orderid:Schema.Types.ObjectId,
        group:String,
        projects:Array,
        username:String,
        operator:String,
        guide:String,
        //报价
        costs:
        {
            placecost:Object,
            mealcost:Object,
            othercost:Object
        },
        visitor:Array,
        //导游的报价
        Actualcosts:
        {
            placecost:Object,
            mealcost:Object,
            othercost:Object
        },
        remark:String
    }
);

//基本信息（导游）
var BaseInfoSchema = new Schema(
    {
        cities:Array,
        groups:Array,
        guides:Array,
        hotels:Array,
        traffic:Array
       // restaurant:Array
    }
);

//操作记录
var LogSchema = new Schema(
    {
        operator:String,
        date:Date,
        orderid:Number,
        operation:String
    }
);

//对应模型对象（与数据库文档关联，可以操作数据库）
//exports.Place = mongoose.model('Place',PlaceSchema);
exports.User = mongoose.model('User', UserSchema);
exports.Operator = mongoose.model('Operator', OpertorSchema);
exports.Order = mongoose.model('Order',OrderSchema);
//exports.Meals = mongoose.model('Meal',MealsSchema);
//exports.Others=mongoose.model('Other',OthersSchema);
exports.Baseinfo = mongoose.model('Baseinfo',BaseInfoSchema);
exports.Log=mongoose.model('Log',LogSchema);

exports.Cost=mongoose.model('Cost',CostSchema);