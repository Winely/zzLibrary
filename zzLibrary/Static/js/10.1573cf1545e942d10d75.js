webpackJsonp([10],{484:function(e,t,o){o(559);var r=o(185)(o(512),o(544),"data-v-715387f8",null);e.exports=r.exports},489:function(e,t,o){"use strict";t.a={root:"http://zz.ngrok.donggu.me/api/",encodeToken:function(e){return e.replace(/\+/g,"%2B")},signup:function(){return this.root+"Account/signup"},login:function(){return this.root+"Account/login"},GetUserInfo:function(e,t){return this.root+"Account/info?token="+this.encodeToken(e)+"&username="+t},GetUserRecord:function(e,t){return this.root+"Record/user?token="+this.encodeToken(e)+"&user="+t},Renew:function(e){return this.root+"Record/renew?token="+this.encodeToken(e)},Borrow:function(e){return this.root+"Record/borrow?token="+this.encodeToken(e)},Return:function(e){return this.root+"Record/return?token="+this.encodeToken(e)},searchBookByISBN:function(e){return this.root+"Book/Get?isbn="+e},searchBookByTitle:function(e,t){return this.root+"Book/search?title="+e+"&page="+t},searchOuterBook:function(e,t){return this.root+"Book/info?token="+this.encodeToken(e)+"&isbn="+t},addBook:function(e){return this.root+"Book/add?token="+this.encodeToken(e)},deleteBook:function(e,t){return this.root+"Book/Delete?token="+this.encodeToken(e)+"&isbn="+t},getCopy:function(e){return this.root+"Copy/Get?isbn="+e},addCopy:function(e,t,o){return this.root+"Copy/add?token="+this.encodeToken(e)+"&isbn="+t+"&num="+o},deleteCopy:function(e,t){return this.root+"Copy/Delete?token="+this.encodeToken(e)+"&id="+t},getCopyRecord:function(e,t){return this.root+"Record/copy?token="+this.encodeToken(e)+"&copyId="+t},getAllRecords:function(e,t){return this.root+"Record/Get?token="+this.encodeToken(e)+"&page="+t},getAllBooks:function(e){return this.root+"Book/Get?page="+e},getAllUsers:function(e){return this.root+"Account/Get?token="+e}}},512:function(e,t,o){"use strict";Object.defineProperty(t,"__esModule",{value:!0});var r=o(489);t.default={props:["admin"],data:function(){return{copyId:null,copyRecords:null,currentPage:1,allRecords:[]}},created:function(){var e=this;this.$http.get(r.a.getAllRecords(this.admin.token,this.currentPage)).then(function(t){e.allRecords=t.body;for(var o=0;o<e.allRecords.records.length;o++)e.allRecords.records[o].isclosed=e.allRecords.records[o].isclosed?"√":"×",e.allRecords.records[o].borrow_time=e.allRecords.records[o].borrow_time.replace("T"," "),e.allRecords.records[o].deadline=e.allRecords.records[o].deadline.replace("T"," ")},function(t){e.allRecords=null})},methods:{search:function(){var e=this;this.$http.get(r.a.getCopyRecord(this.admin.token,this.copyId)).then(function(t){e.copyRecords=t.body;for(var o=0;o<e.copyRecords.length;o++)e.copyRecords[o].isclosed=e.copyRecords[o].isclosed?"√":"×",e.copyRecords[o].borrow_time=e.copyRecords[o].borrow_time.replace("T"," "),e.copyRecords[o].deadline=e.copyRecords[o].deadline.replace("T"," ")},function(t){e.copyRecords=null})},handleCurrentChange:function(e){var t=this;this.currentPage=e,this.$http.get(r.a.getAllRecords(this.admin.token,this.currentPage)).then(function(e){t.allRecords=e.body;for(var o=0;o<t.allRecords.records.length;o++)console.log(o),t.allRecords.records[o].isclosed=t.allRecords.records[o].isclosed?"√":"×",t.allRecords.records[o].borrow_time=t.allRecords.records[o].borrow_time.replace("T"," "),t.allRecords.records[o].deadline=t.allRecords.records[o].deadline.replace("T"," ")},function(e){t.allRecords=null})}}}},527:function(e,t,o){t=e.exports=o(82)(void 0),t.push([e.i,".userInfo[data-v-715387f8]{margin-top:20px;border-left:6px solid #eee;padding:10px;margin-bottom:20px}.crumbs[data-v-715387f8]{margin:10px 0}.table-title[data-v-715387f8]{font-size:20px}.search-margin[data-v-715387f8]{margin-top:10px;margin-bottom:20px}",""])},544:function(e,t){e.exports={render:function(){var e=this,t=e.$createElement,o=e._self._c||t;return o("div",[o("div",{staticClass:"crumbs"},[o("el-breadcrumb",{attrs:{separator:"/"}},[o("el-breadcrumb-item",{staticClass:"table-title"},[o("i",{staticClass:"el-icon-menu table-title"}),e._v(" 复本借阅记录查询\n            ")])],1)],1),e._v(" "),o("el-input",{staticStyle:{width:"200px"},attrs:{type:"text",placeholder:"请输入复本ID"},model:{value:e.copyId,callback:function(t){e.copyId=t},expression:"copyId"}}),e._v(" "),o("el-button",{staticClass:"search-margin",attrs:{type:"primary"},on:{click:function(t){e.search()}}},[e._v("查询")]),e._v(" "),e.copyRecords?o("el-table",{ref:"multipleTable",staticStyle:{width:"912px"},attrs:{data:e.copyRecords,border:""}},[o("el-table-column",{attrs:{prop:"book",label:"书名",sortable:"",width:"200"}}),e._v(" "),o("el-table-column",{attrs:{prop:"borrow_time",label:"借书日期",sortable:"",width:"170"}}),e._v(" "),o("el-table-column",{attrs:{prop:"deadline",label:"还书日期",sortable:"",width:"170"}}),e._v(" "),o("el-table-column",{attrs:{prop:"renew",label:"剩余续借次数",sortable:"",width:"150"}}),e._v(" "),o("el-table-column",{attrs:{prop:"isclosed",label:"是否归还",sortable:"",width:"120"}}),e._v(" "),o("el-table-column",{attrs:{prop:"operator",label:"经手人",sortable:"",width:"100"}})],1):e._e(),e._v(" "),e.allRecords&&!e.copyRecords?o("div",[o("div",{staticClass:"crumbs"},[o("el-breadcrumb",{attrs:{separator:"/"}},[o("el-breadcrumb-item",{staticClass:"table-title"},[o("i",{staticClass:"el-icon-menu table-title"}),e._v(" 全部借阅记录\n                ")])],1)],1),e._v(" "),o("el-table",{ref:"multipleTable",staticStyle:{width:"100%","margin-top":"20px"},attrs:{data:e.allRecords.records,border:""}},[o("el-table-column",{attrs:{prop:"copy",label:"复本id",sortable:"",width:"200"}}),e._v(" "),o("el-table-column",{attrs:{prop:"user",label:"借书人",sortable:"",width:"170"}}),e._v(" "),o("el-table-column",{attrs:{prop:"borrow_time",label:"借书日期",sortable:"",width:"170"}}),e._v(" "),o("el-table-column",{attrs:{prop:"deadline",label:"还书日期",sortable:"",width:"170"}}),e._v(" "),o("el-table-column",{attrs:{prop:"isclosed",label:"是否归还",sortable:"",width:"120"}}),e._v(" "),o("el-table-column",{attrs:{prop:"operator",label:"经手人",sortable:"",width:"100"}})],1),e._v(" "),o("div",{staticClass:"block"},[o("el-pagination",{attrs:{"current-page":e.currentPage,"page-size":40,layout:"prev, pager, next, jumper",total:40*e.allRecords.total},on:{"current-change":e.handleCurrentChange}})],1)],1):e._e()],1)},staticRenderFns:[]}},559:function(e,t,o){var r=o(527);"string"==typeof r&&(r=[[e.i,r,""]]),r.locals&&(e.exports=r.locals);o(186)("4f3473ae",r,!0)}});