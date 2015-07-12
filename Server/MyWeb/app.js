
/**
 * Module dependencies.
 */
//session 支持
var express = require('express');
var MongoStore = require('connect-mongo')(express);
var routes = require('./routes');

var regUser = require('./routes/RegUser');
var Routeplanlist = require('./routes/PlanList');
var RoutePlanView = require('./routes/PlanView');
var RouteFindPlan = require('./routes/FindPlan');

var http = require('http');
var path = require('path');
var RouteLogin=require('./routes/Login');
var RouteOrder=require('./routes/Order');
var RouteProject=require('./routes/Project');
var RouteCost=require('./routes/Cost');
var RouteBridge=require('./routes/Bridge');

var app = express();

var partials = require('express-partials');
var flash = require('connect-flash');
var settings = require('./settings');
var sessionStore = new MongoStore({
    db : settings.db
}, function() {
    console.log('connect mongodb success...');
});
app.configure(function(){
    app.set('port', process.env.PORT || 3000);
    app.set('views', __dirname + '/views');
    app.set('view engine', 'ejs');

    app.use(partials());
    app.use(flash());

    app.use(express.favicon());
    app.use(express.logger('dev'));
    //app.use(express.bodyParser());
    app.use(express.json());
    app.use(express.urlencoded());
    app.use(express.methodOverride());

    app.use(express.cookieParser());

    app.use(express.session({
        secret : settings.cookie_secret,
        cookie : {
            maxAge : 60000 * 20	//20 minutes
        },
        store : sessionStore
    }));

    app.use(app.router);
    app.use(express.static(__dirname + '/public'));
});


// all environments
app.set('port', process.env.PORT || 3000);
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'ejs');
app.use(express.favicon());
app.use(express.logger('dev'));
app.use(express.json());
app.use(express.urlencoded());
app.use(express.methodOverride());
app.use(app.router);
app.use(express.static(path.join(__dirname, 'public')));

// development onlys
if ('development' == app.get('env')) {
  app.use(express.errorHandler());
}


/////////////////////////////////////路由管理
app.get('/', routes.index);

app.get('/RegUser', regUser.regNewUser);


//登陆

app.get('/PlanList', Routeplanlist.planList);       // 计划单列表
app.get('/PlanView', RoutePlanView.planView);       // 计划单展示
app.get('/FindPlan', RouteFindPlan.Find);       // 查找计划单
app.post('/FindPlan', RouteFindPlan.FindPlan);       // 查找计划单


//添加计划单

app.get('/NewOrder',RouteOrder.NewOrder);

//添加单日的行程
app.post('/AddDailyPlan',RouteOrder.AddDailyPlan);
//添加整个计划单
app.post('/SavePlan',RouteOrder.SavePlan);


//编辑计划单

app.get('/EditorOrder',RouteOrder.EditorOrder);
app.post('/postEditorOrder',RouteOrder.postEditorOrder);

app.post('/EditorDailyPlan',RouteBridge.EditorDailyPlan);
app.post('/doEditorDailyPlan',RouteBridge.doEditorDailyPlan);






//显示结算单
app.get('/ShowCost',RouteCost.ShowCost);
//app.get('/doEditorCost',RouteCost.doEditorCost);
//app.post('/postEditorCost',RouteCost.postEditorCost);





//修改成本单
app.get('/EditorActualCost',RouteCost.EditorActualCost);
app.get('/doEditorActualCost',RouteCost.doEditorActualCost);
app.post('/postEditorActualCost',RouteCost.postEditorActualCost);




//汇总
app.get('/CostSummary',RouteCost.CostSummary);
app.get('/doCostSummary',RouteCost.doCostSummary);







////////////////////////////////////////////////////////////////
app.post('/login',RouteLogin.Login);
http.createServer(app).listen(app.get('port'), function()
{
  console.log('Express server listening on port ' + app.get('port'));
});





