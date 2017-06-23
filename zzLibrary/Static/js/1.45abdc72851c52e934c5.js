webpackJsonp([1],{189:function(o,e,t){t(555);var r=t(185)(t(510),t(540),"data-v-482021d0",null);o.exports=r.exports},489:function(o,e,t){"use strict";e.a={root:"/api/",encodeToken:function(o){return o.replace(/\+/g,"%2B")},signup:function(){return this.root+"Account/signup"},login:function(){return this.root+"Account/login"},GetUserInfo:function(o,e){return this.root+"Account/info?token="+this.encodeToken(o)+"&username="+e},GetUserRecord:function(o,e){return this.root+"Record/user?token="+this.encodeToken(o)+"&user="+e},Renew:function(o){return this.root+"Record/renew?token="+this.encodeToken(o)},Borrow:function(o){return this.root+"Record/borrow?token="+this.encodeToken(o)},Return:function(o){return this.root+"Record/return?token="+this.encodeToken(o)},searchBookByISBN:function(o){return this.root+"Book/Get?isbn="+o},searchBookByTitle:function(o,e){return this.root+"Book/search?title="+o+"&page="+e},searchOuterBook:function(o,e){return this.root+"Book/info?token="+this.encodeToken(o)+"&isbn="+e},addBook:function(o){return this.root+"Book/add?token="+this.encodeToken(o)},deleteBook:function(o,e){return this.root+"Book/Delete?token="+this.encodeToken(o)+"&isbn="+e},getCopy:function(o){return this.root+"Copy/Get?isbn="+o},addCopy:function(o,e,t){return this.root+"Copy/add?token="+this.encodeToken(o)+"&isbn="+e+"&num="+t},deleteCopy:function(o,e){return this.root+"Copy/Delete?token="+this.encodeToken(o)+"&id="+e},getCopyRecord:function(o,e){return this.root+"Record/copy?token="+this.encodeToken(o)+"&copyId="+e},getAllRecords:function(o,e){return this.root+"Record/Get?token="+this.encodeToken(o)+"&page="+e},getAllBooks:function(o){return this.root+"Book/Get?page="+o},getAllUsers:function(o){return this.root+"Account/Get?token="+o}}},510:function(o,e,t){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var r=t(489);e.default={props:["admin"],data:function(){return{penalty:null,dialogVisible:!1,borrowBook:{b_Username:null,b_copy:null},returnBook:{r_Username:null,r_copy:null}}},created:function(){},methods:{submitBorrow:function(o){var e=this;this.$refs[o].validate(function(o){if(!o)return!1;e.$http.post(r.a.Borrow(e.admin.token),{username:e.borrowBook.b_Username,copy:e.borrowBook.b_copy}).then(function(o){e.$message({message:"借书成功！",type:"success"}),e.borrowBook.b_Username=null,e.borrowBook.b_copy=null,e.returnBook.r_Username=null,e.returnBook.r_copy=null},function(o){e.$message({message:"借书失败！",type:"error"})})})},resetBorrow:function(o){this.$refs[o].resetFields()},submitReturn:function(o){var e=this;this.$refs[o].validate(function(o){if(!o)return!1;e.$http.post(r.a.Return(e.admin.token),{username:e.returnBook.r_Username,copy:e.returnBook.r_copy}).then(function(o){e.borrowBook.b_Username=null,e.borrowBook.b_copy=null,e.returnBook.r_Username=null,e.returnBook.r_copy=null,e.$message({message:"还书成功！",type:"success"}),e.dialogVisible=!0,e.penalty=o.body.penalty,console.log(e.penalty)},function(o){e.$message({message:"还书失败！",type:"error"})})})},resetReturn:function(o){this.$refs[o].resetFields()}}}},523:function(o,e,t){e=o.exports=t(82)(void 0),e.push([o.i,".crumbs[data-v-482021d0]{margin:10px 0}.table-title[data-v-482021d0]{font-size:20px}",""])},540:function(o,e){o.exports={render:function(){var o=this,e=o.$createElement,t=o._self._c||e;return t("div",{staticClass:"BRBook-wrap"},[t("div",{staticClass:"crumbs"},[t("el-breadcrumb",{attrs:{separator:"/"}},[t("el-breadcrumb-item",{staticClass:"table-title"},[t("i",{staticClass:"el-icon-menu table-title"}),o._v(" 借书\n            ")])],1)],1),o._v(" "),t("div",{staticClass:"form-box"},[t("el-form",{ref:"borrowBook",attrs:{model:o.borrowBook,"label-width":"100px"}},[t("el-form-item",{attrs:{label:"用户名",prop:"b_Username"}},[t("el-input",{model:{value:o.borrowBook.b_Username,callback:function(e){o.borrowBook.b_Username=e},expression:"borrowBook.b_Username"}})],1),o._v(" "),t("el-form-item",{attrs:{label:"复本id",prop:"b_copy"}},[t("el-input",{attrs:{"auto-complete":"off"},model:{value:o.borrowBook.b_copy,callback:function(e){o.borrowBook.b_copy=e},expression:"borrowBook.b_copy"}})],1),o._v(" "),t("el-form-item",[t("el-button",{attrs:{type:"primary"},on:{click:function(e){o.submitBorrow("borrowBook")}}},[o._v("提交")]),o._v(" "),t("el-button",{on:{click:function(e){o.resetBorrow("borrowBook")}}},[o._v("重置")])],1)],1)],1),o._v(" "),t("div",{staticClass:"crumbs"},[t("el-breadcrumb",{attrs:{separator:"/"}},[t("el-breadcrumb-item",{staticClass:"table-title"},[t("i",{staticClass:"el-icon-menu table-title"}),o._v(" 还书\n            ")])],1)],1),o._v(" "),t("div",{staticClass:"form-box"},[t("el-form",{ref:"returnBook",attrs:{model:o.returnBook,"label-width":"100px"}},[t("el-form-item",{attrs:{label:"用户名",prop:"r_Username"}},[t("el-input",{model:{value:o.returnBook.r_Username,callback:function(e){o.returnBook.r_Username=e},expression:"returnBook.r_Username"}})],1),o._v(" "),t("el-form-item",{attrs:{label:"复本id",prop:"r_copy"}},[t("el-input",{attrs:{"auto-complete":"off"},model:{value:o.returnBook.r_copy,callback:function(e){o.returnBook.r_copy=e},expression:"returnBook.r_copy"}})],1),o._v(" "),t("el-form-item",[t("el-button",{attrs:{type:"primary"},on:{click:function(e){o.submitReturn("returnBook")}}},[o._v("提交")]),o._v(" "),t("el-button",{on:{click:function(e){o.resetReturn("returnBook")}}},[o._v("重置")])],1)],1)],1),o._v(" "),t("el-dialog",{attrs:{title:"提示",visible:o.dialogVisible,size:"tiny"},on:{"update:visible":function(e){o.dialogVisible=e}}},[t("span",[o._v("还书成功")]),o._v(" "),t("p",[o._v("欠款："),t("span",[o._v(o._s(o.penalty))]),o._v("元")]),o._v(" "),t("span",{staticClass:"dialog-footer",slot:"footer"},[t("el-button",{on:{click:function(e){o.dialogVisible=!1}}},[o._v("取 消")]),o._v(" "),t("el-button",{attrs:{type:"primary"},on:{click:function(e){o.dialogVisible=!1}}},[o._v("确 定")])],1)])],1)},staticRenderFns:[]}},555:function(o,e,t){var r=t(523);"string"==typeof r&&(r=[[o.i,r,""]]),r.locals&&(o.exports=r.locals);t(186)("3859da52",r,!0)}});