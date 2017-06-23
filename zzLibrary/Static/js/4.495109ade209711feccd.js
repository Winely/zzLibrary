webpackJsonp([4],{

/***/ 480:
/***/ (function(module, exports, __webpack_require__) {


/* styles */
__webpack_require__(557)

var Component = __webpack_require__(185)(
  /* script */
  __webpack_require__(507),
  /* template */
  __webpack_require__(542),
  /* scopeId */
  "data-v-682f282a",
  /* cssModules */
  null
)

module.exports = Component.exports


/***/ }),

/***/ 489:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony default export */ __webpack_exports__["a"] = ({
    root: 'http://zz.ngrok.donggu.me/api/',
    encodeToken: function encodeToken(token) {
        return token.replace(/\+/g, '%2B');
    },
    signup: function signup() {
        return this.root + 'Account/signup';
    },
    login: function login() {
        return this.root + 'Account/login';
    },
    GetUserInfo: function GetUserInfo(token, username) {
        return this.root + 'Account/info?token=' + this.encodeToken(token) + '&username=' + username;
    },
    GetUserRecord: function GetUserRecord(token, username) {
        return this.root + 'Record/user?token=' + this.encodeToken(token) + '&user=' + username;
    },
    Renew: function Renew(token) {
        return this.root + 'Record/renew?token=' + this.encodeToken(token);
    },
    Borrow: function Borrow(token) {
        return this.root + 'Record/borrow?token=' + this.encodeToken(token);
    },
    Return: function Return(token) {
        return this.root + 'Record/return?token=' + this.encodeToken(token);
    },
    searchBookByISBN: function searchBookByISBN(isbn) {
        return this.root + 'Book/Get?isbn=' + isbn;
    },
    searchBookByTitle: function searchBookByTitle(title, pageid) {
        return this.root + 'Book/search?title=' + title + '&page=' + pageid;
    },
    searchOuterBook: function searchOuterBook(token, isbn) {
        return this.root + 'Book/info?token=' + this.encodeToken(token) + '&isbn=' + isbn;
    },
    addBook: function addBook(token) {
        return this.root + 'Book/add?token=' + this.encodeToken(token);
    },
    deleteBook: function deleteBook(token, isbn) {
        return this.root + 'Book/Delete?token=' + this.encodeToken(token) + '&isbn=' + isbn;
    },
    getCopy: function getCopy(isbn) {
        return this.root + 'Copy/Get?isbn=' + isbn;
    },
    addCopy: function addCopy(token, isbn, num) {
        return this.root + 'Copy/add?token=' + this.encodeToken(token) + '&isbn=' + isbn + '&num=' + num;
    },
    deleteCopy: function deleteCopy(token, id) {
        return this.root + 'Copy/Delete?token=' + this.encodeToken(token) + '&id=' + id;
    },
    getCopyRecord: function getCopyRecord(token, id) {
        return this.root + 'Record/copy?token=' + this.encodeToken(token) + '&copyId=' + id;
    },
    getAllRecords: function getAllRecords(token, pageid) {
        return this.root + 'Record/Get?token=' + this.encodeToken(token) + '&page=' + pageid;
    },
    getAllBooks: function getAllBooks(pageid) {
        return this.root + 'Book/Get?page=' + pageid;
    },
    getAllUsers: function getAllUsers(token) {
        return this.root + 'Account/Get?token=' + token;
    }
});

/***/ }),

/***/ 502:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_async_validator__ = __webpack_require__(188);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_element_ui_src_mixins_emitter__ = __webpack_require__(530);





function noop() {}

function getPropByPath(obj, path) {
  var tempObj = obj;
  path = path.replace(/\[(\w+)\]/g, '.$1');
  path = path.replace(/^\./, '');

  var keyArr = path.split('.');
  var i = 0;

  for (var len = keyArr.length; i < len - 1; ++i) {
    var key = keyArr[i];
    if (key in tempObj) {
      tempObj = tempObj[key];
    } else {
      throw new Error('please transfer a valid prop path to form item!');
    }
  }
  return {
    o: tempObj,
    k: keyArr[i],
    v: tempObj[keyArr[i]]
  };
}

/* harmony default export */ __webpack_exports__["default"] = ({
  name: 'ElFormItem',

  componentName: 'ElFormItem',

  mixins: [__WEBPACK_IMPORTED_MODULE_1_element_ui_src_mixins_emitter__["a" /* default */]],

  props: {
    label: String,
    labelWidth: String,
    prop: String,
    required: Boolean,
    rules: [Object, Array],
    error: String,
    validateStatus: String,
    showMessage: {
      type: Boolean,
      default: true
    }
  },
  watch: {
    error: function error(value) {
      this.validateMessage = value;
      this.validateState = value ? 'error' : '';
    },
    validateStatus: function validateStatus(value) {
      this.validateState = value;
    }
  },
  computed: {
    labelStyle: function labelStyle() {
      var ret = {};
      if (this.form.labelPosition === 'top') return ret;
      var labelWidth = this.labelWidth || this.form.labelWidth;
      if (labelWidth) {
        ret.width = labelWidth;
      }
      return ret;
    },
    contentStyle: function contentStyle() {
      var ret = {};
      if (this.form.labelPosition === 'top' || this.form.inline) return ret;
      var labelWidth = this.labelWidth || this.form.labelWidth;
      if (labelWidth) {
        ret.marginLeft = labelWidth;
      }
      return ret;
    },
    form: function form() {
      var parent = this.$parent;
      while (parent.$options.componentName !== 'ElForm') {
        parent = parent.$parent;
      }
      return parent;
    },

    fieldValue: {
      cache: false,
      get: function get() {
        var model = this.form.model;
        if (!model || !this.prop) {
          return;
        }

        var path = this.prop;
        if (path.indexOf(':') !== -1) {
          path = path.replace(/:/, '.');
        }

        return getPropByPath(model, path).v;
      }
    }
  },
  data: function data() {
    return {
      validateState: '',
      validateMessage: '',
      validateDisabled: false,
      validator: {},
      isRequired: false
    };
  },

  methods: {
    validate: function validate(trigger) {
      var _this = this;

      var callback = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : noop;

      var rules = this.getFilteredRule(trigger);
      if (!rules || rules.length === 0) {
        callback();
        return true;
      }

      this.validateState = 'validating';

      var descriptor = {};
      descriptor[this.prop] = rules;

      var validator = new __WEBPACK_IMPORTED_MODULE_0_async_validator__["default"](descriptor);
      var model = {};

      model[this.prop] = this.fieldValue;

      validator.validate(model, { firstFields: true }, function (errors, fields) {
        _this.validateState = !errors ? 'success' : 'error';
        _this.validateMessage = errors ? errors[0].message : '';

        callback(_this.validateMessage);
      });
    },
    resetField: function resetField() {
      this.validateState = '';
      this.validateMessage = '';

      var model = this.form.model;
      var value = this.fieldValue;
      var path = this.prop;
      if (path.indexOf(':') !== -1) {
        path = path.replace(/:/, '.');
      }

      var prop = getPropByPath(model, path);

      if (Array.isArray(value)) {
        this.validateDisabled = true;
        prop.o[prop.k] = [].concat(this.initialValue);
      } else {
        this.validateDisabled = true;
        prop.o[prop.k] = this.initialValue;
      }
    },
    getRules: function getRules() {
      var formRules = this.form.rules;
      var selfRuels = this.rules;

      formRules = formRules ? formRules[this.prop] : [];

      return [].concat(selfRuels || formRules || []);
    },
    getFilteredRule: function getFilteredRule(trigger) {
      var rules = this.getRules();

      return rules.filter(function (rule) {
        return !rule.trigger || rule.trigger.indexOf(trigger) !== -1;
      });
    },
    onFieldBlur: function onFieldBlur() {
      this.validate('blur');
    },
    onFieldChange: function onFieldChange() {
      if (this.validateDisabled) {
        this.validateDisabled = false;
        return;
      }

      this.validate('change');
    }
  },
  mounted: function mounted() {
    var _this2 = this;

    if (this.prop) {
      this.dispatch('ElForm', 'el.form.addField', [this]);

      var initialValue = this.fieldValue;
      if (Array.isArray(initialValue)) {
        initialValue = [].concat(initialValue);
      }
      Object.defineProperty(this, 'initialValue', {
        value: initialValue
      });

      var rules = this.getRules();

      if (rules.length) {
        rules.every(function (rule) {
          if (rule.required) {
            _this2.isRequired = true;
            return false;
          }
        });
        this.$on('el.form.blur', this.onFieldBlur);
        this.$on('el.form.change', this.onFieldChange);
      }
    }
  },
  beforeDestroy: function beforeDestroy() {
    this.dispatch('ElForm', 'el.form.removeField', [this]);
  }
});

/***/ }),

/***/ 507:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_assets_url_conf__ = __webpack_require__(489);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__node_modules_element_ui_packages_form_src_form_item__ = __webpack_require__(531);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__node_modules_element_ui_packages_form_src_form_item___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1__node_modules_element_ui_packages_form_src_form_item__);




/* harmony default export */ __webpack_exports__["default"] = ({
    components: { ElFormItem: __WEBPACK_IMPORTED_MODULE_1__node_modules_element_ui_packages_form_src_form_item___default.a },
    props: ['admin'],
    data: function data() {
        return {
            isbn: null,
            bookInfo: null,
            hasBookInfo: null,
            errorCode: false
        };
    },
    created: function created() {},

    methods: {
        search: function search() {
            var _this = this;

            this.$http.get(__WEBPACK_IMPORTED_MODULE_0_assets_url_conf__["a" /* default */].searchBookByISBN(this.isbn)).then(function (resp) {
                _this.hasBookInfo = resp.body;
                _this.errorCode = false;
            }, function (resp) {
                _this.hasBookInfo = null;
                _this.$http.get(__WEBPACK_IMPORTED_MODULE_0_assets_url_conf__["a" /* default */].searchOuterBook(_this.admin.token, _this.isbn)).then(function (response) {
                    _this.bookInfo = response.body;
                    if (_this.bookInfo.hasOwnProperty("ErrorCode")) {
                        _this.errorCode = true;
                    } else {
                        _this.errorCode = false;
                    }
                }, function (resp) {
                    _this.bookInfo = null;
                });
            });
        },
        addBook: function addBook() {
            var _this2 = this;

            var info = {
                "isbn": this.bookInfo.ISBN,
                "title": this.bookInfo.BookName,
                "author": this.bookInfo.Author,
                "price": this.bookInfo.Price,
                "edition": this.bookInfo.Publishing,
                "copy": null
            };
            this.$http.put(__WEBPACK_IMPORTED_MODULE_0_assets_url_conf__["a" /* default */].addBook(this.admin.token), info).then(function (resp) {
                _this2.$message({
                    message: '录入成功！',
                    type: 'success'
                });
                _this2.bookInfo = null;
                _this2.isbn = null;
            }, function (resp) {
                _this2.$message({
                    message: '录入失败！',
                    type: 'error'
                });
            });
        },
        deleteBook: function deleteBook() {
            var _this3 = this;

            this.$http.delete(__WEBPACK_IMPORTED_MODULE_0_assets_url_conf__["a" /* default */].deleteBook(this.admin.token, this.isbn)).then(function (resp) {
                _this3.$message({
                    message: '删除成功！',
                    type: 'success'
                });
                _this3.hasBookInfo = null;
                _this3.isbn = null;
            }, function (resp) {
                _this3.$message({
                    message: '删除失败！',
                    type: 'error'
                });
            });
        }
    }
});

/***/ }),

/***/ 525:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(82)(undefined);
// imports


// module
exports.push([module.i, ".crumbs[data-v-682f282a]{margin:10px 0}.table-title[data-v-682f282a]{font-size:20px}.bookInfoForm[data-v-682f282a]{margin-top:30px}.tip[data-v-682f282a]{margin-left:20px;font-size:17px;color:#d43f3a}", ""]);

// exports


/***/ }),

/***/ 530:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
function broadcast(componentName, eventName, params) {
  this.$children.forEach(child => {
    var name = child.$options.componentName;

    if (name === componentName) {
      child.$emit.apply(child, [eventName].concat(params));
    } else {
      broadcast.apply(child, [componentName, eventName].concat([params]));
    }
  });
}
/* harmony default export */ __webpack_exports__["a"] = ({
  methods: {
    dispatch(componentName, eventName, params) {
      var parent = this.$parent || this.$root;
      var name = parent.$options.componentName;

      while (parent && (!name || name !== componentName)) {
        parent = parent.$parent;

        if (parent) {
          name = parent.$options.componentName;
        }
      }
      if (parent) {
        parent.$emit.apply(parent, [eventName].concat(params));
      }
    },
    broadcast(componentName, eventName, params) {
      broadcast.call(this, componentName, eventName, params);
    }
  }
});


/***/ }),

/***/ 531:
/***/ (function(module, exports, __webpack_require__) {

var Component = __webpack_require__(185)(
  /* script */
  __webpack_require__(502),
  /* template */
  __webpack_require__(547),
  /* scopeId */
  null,
  /* cssModules */
  null
)

module.exports = Component.exports


/***/ }),

/***/ 542:
/***/ (function(module, exports) {

module.exports={render:function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('div', {
    staticClass: "BRBook-wrap"
  }, [_c('div', {
    staticClass: "crumbs"
  }, [_c('el-breadcrumb', {
    attrs: {
      "separator": "/"
    }
  }, [_c('el-breadcrumb-item', {
    staticClass: "table-title"
  }, [_c('i', {
    staticClass: "el-icon-menu table-title"
  }), _vm._v(" 录入/删除书本\n            ")])], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "bookInfoForm"
  }, [_c('div', {
    staticClass: "form-box"
  }, [_c('el-form', {
    attrs: {
      "label-width": "40px"
    }
  }, [_c('el-form-item', {
    attrs: {
      "label": "ISBN",
      "prop": "isbn"
    }
  }, [_c('el-input', {
    staticStyle: {
      "width": "200px"
    },
    attrs: {
      "placeholder": "输入书本的ISBN"
    },
    model: {
      value: (_vm.isbn),
      callback: function($$v) {
        _vm.isbn = $$v
      },
      expression: "isbn"
    }
  }), _vm._v(" "), _c('el-button', {
    attrs: {
      "type": "primary"
    },
    on: {
      "click": function($event) {
        _vm.search()
      }
    }
  }, [_vm._v("查询")]), _vm._v(" "), (_vm.hasBookInfo) ? _c('el-p', {
    staticClass: "tip"
  }, [_vm._v("馆内有此书")]) : _vm._e(), _vm._v(" "), (!this.hasBookInfo && this.bookInfo && !this.bookInfo.hasOwnProperty('ErrorCode')) ? _c('el-p', {
    staticClass: "tip"
  }, [_vm._v("馆内无此书，可录入")]) : _vm._e(), _vm._v(" "), (!this.hasBookInfo && this.bookInfo && this.bookInfo.hasOwnProperty('ErrorCode')) ? _c('el-p', {
    staticClass: "tip"
  }, [_vm._v("此ISBN无效")]) : _vm._e()], 1)], 1), _vm._v(" "), (_vm.hasBookInfo) ? _c('el-form', {
    ref: "hasBookInfo",
    attrs: {
      "model": _vm.hasBookInfo,
      "label-width": "40px"
    }
  }, [_c('el-form-item', {
    attrs: {
      "label": "书名",
      "prop": "title"
    }
  }, [_c('p', [_vm._v(_vm._s(_vm.hasBookInfo.title))])]), _vm._v(" "), _c('el-form-item', {
    attrs: {
      "label": "作者",
      "prop": "author"
    }
  }, [_c('p', [_vm._v(_vm._s(_vm.hasBookInfo.author))])]), _vm._v(" "), _c('el-form-item', {
    attrs: {
      "label": "版本",
      "prop": "edition"
    }
  }, [_c('p', [_vm._v(_vm._s(_vm.hasBookInfo.edition))])]), _vm._v(" "), _c('el-form-item', {
    attrs: {
      "label": "价钱",
      "prop": "price"
    }
  }, [_c('p', [_vm._v(_vm._s(_vm.hasBookInfo.price))])]), _vm._v(" "), _c('el-form-item', [_c('el-button', {
    attrs: {
      "type": "danger"
    },
    on: {
      "click": function($event) {
        _vm.deleteBook()
      }
    }
  }, [_vm._v("删除")])], 1)], 1) : _vm._e(), _vm._v(" "), (!this.hasBookInfo && this.bookInfo && !this.bookInfo.hasOwnProperty('ErrorCode')) ? _c('el-form', {
    ref: "bookInfo",
    attrs: {
      "model": _vm.bookInfo,
      "label-width": "40px"
    }
  }, [_c('el-form-item', {
    attrs: {
      "label": "书名",
      "prop": "BookName"
    }
  }, [_c('el-input', {
    model: {
      value: (_vm.bookInfo.BookName),
      callback: function($$v) {
        _vm.bookInfo.BookName = $$v
      },
      expression: "bookInfo.BookName"
    }
  })], 1), _vm._v(" "), _c('el-form-item', {
    attrs: {
      "label": "作者",
      "prop": "Author"
    }
  }, [_c('el-input', {
    model: {
      value: (_vm.bookInfo.Author),
      callback: function($$v) {
        _vm.bookInfo.Author = $$v
      },
      expression: "bookInfo.Author"
    }
  })], 1), _vm._v(" "), _c('el-form-item', {
    attrs: {
      "label": "版本",
      "prop": "Publishing"
    }
  }, [_c('el-input', {
    model: {
      value: (_vm.bookInfo.Publishing),
      callback: function($$v) {
        _vm.bookInfo.Publishing = $$v
      },
      expression: "bookInfo.Publishing"
    }
  })], 1), _vm._v(" "), _c('el-form-item', {
    attrs: {
      "label": "价钱",
      "prop": "Price"
    }
  }, [_c('el-input', {
    model: {
      value: (_vm.bookInfo.Price),
      callback: function($$v) {
        _vm.bookInfo.Price = $$v
      },
      expression: "bookInfo.Price"
    }
  })], 1), _vm._v(" "), _c('el-form-item', [_c('el-button', {
    attrs: {
      "type": "success"
    },
    on: {
      "click": function($event) {
        _vm.addBook()
      }
    }
  }, [_vm._v("录入")])], 1)], 1) : _vm._e()], 1)])])
},staticRenderFns: []}

/***/ }),

/***/ 547:
/***/ (function(module, exports) {

module.exports={render:function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('div', {
    staticClass: "el-form-item",
    class: {
      'is-error': _vm.validateState === 'error',
        'is-validating': _vm.validateState === 'validating',
        'is-required': _vm.isRequired || _vm.required
    }
  }, [(_vm.label) ? _c('label', {
    staticClass: "el-form-item__label",
    style: (_vm.labelStyle),
    attrs: {
      "for": _vm.prop
    }
  }, [_vm._t("label", [_vm._v(_vm._s(_vm.label + _vm.form.labelSuffix))])], 2) : _vm._e(), _vm._v(" "), _c('div', {
    staticClass: "el-form-item__content",
    style: (_vm.contentStyle)
  }, [_vm._t("default"), _vm._v(" "), _c('transition', {
    attrs: {
      "name": "el-zoom-in-top"
    }
  }, [(_vm.validateState === 'error' && _vm.showMessage && _vm.form.showMessage) ? _c('div', {
    staticClass: "el-form-item__error"
  }, [_vm._v(_vm._s(_vm.validateMessage))]) : _vm._e()])], 2)])
},staticRenderFns: []}

/***/ }),

/***/ 557:
/***/ (function(module, exports, __webpack_require__) {

// style-loader: Adds some css to the DOM by adding a <style> tag

// load the styles
var content = __webpack_require__(525);
if(typeof content === 'string') content = [[module.i, content, '']];
if(content.locals) module.exports = content.locals;
// add the styles to the DOM
var update = __webpack_require__(186)("3dcf0986", content, true);

/***/ })

});