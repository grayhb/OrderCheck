/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is not neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./vue-src/layouts/register.vue":
/*!**************************************!*\
  !*** ./vue-src/layouts/register.vue ***!
  \**************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => __WEBPACK_DEFAULT_EXPORT__\n/* harmony export */ });\n/* harmony import */ var _register_vue_vue_type_template_id_6b107229_scoped_true___WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./register.vue?vue&type=template&id=6b107229&scoped=true& */ \"./vue-src/layouts/register.vue?vue&type=template&id=6b107229&scoped=true&\");\n/* harmony import */ var _register_vue_vue_type_script_lang_js___WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./register.vue?vue&type=script&lang=js& */ \"./vue-src/layouts/register.vue?vue&type=script&lang=js&\");\n/* harmony import */ var _node_modules_vue_loader_lib_runtime_componentNormalizer_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! !../../node_modules/vue-loader/lib/runtime/componentNormalizer.js */ \"./node_modules/vue-loader/lib/runtime/componentNormalizer.js\");\n\n\n\n\n\n/* normalize component */\n;\nvar component = (0,_node_modules_vue_loader_lib_runtime_componentNormalizer_js__WEBPACK_IMPORTED_MODULE_2__.default)(\n  _register_vue_vue_type_script_lang_js___WEBPACK_IMPORTED_MODULE_1__.default,\n  _register_vue_vue_type_template_id_6b107229_scoped_true___WEBPACK_IMPORTED_MODULE_0__.render,\n  _register_vue_vue_type_template_id_6b107229_scoped_true___WEBPACK_IMPORTED_MODULE_0__.staticRenderFns,\n  false,\n  null,\n  \"6b107229\",\n  null\n  \n)\n\n/* hot reload */\nif (false) { var api; }\ncomponent.options.__file = \"vue-src/layouts/register.vue\"\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (component.exports);\n\n//# sourceURL=webpack://order-check/./vue-src/layouts/register.vue?");

/***/ }),

/***/ "./node_modules/vue-loader/lib/index.js??vue-loader-options!./vue-src/layouts/register.vue?vue&type=script&lang=js&":
/*!**************************************************************************************************************************!*\
  !*** ./node_modules/vue-loader/lib/index.js??vue-loader-options!./vue-src/layouts/register.vue?vue&type=script&lang=js& ***!
  \**************************************************************************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => __WEBPACK_DEFAULT_EXPORT__\n/* harmony export */ });\n/* harmony import */ var _helpers_jwt_helpers__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../helpers/jwt-helpers */ \"./vue-src/helpers/jwt-helpers.js\");\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n//\n\n\n\n\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ({\n    created: function () {\n        this.$vuetify.lang.current = 'ru';\n    },\n\n    data: () => ({\n\n        valid: false,\n        showPassword: false,\n        loading:false,\n\n        email: '',\n        displayName: '',\n        password: '',\n\n        rules: {\n            required: value => !!value || 'Обязательное поле',\n            password: v => /^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{6,}$/.test(v) || 'Минимум 6 символов, минимум одна буква, одна цифра и один специальный символ',\n            email: value => {\n                const pattern = /^(([^<>()[\\]\\\\.,;:\\s@\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$/\n                return pattern.test(value) || 'Недопустимый email'\n            },\n        },\n\n        snackbar: false,\n        snackbarText: '',\n        timeout: 15000,\n\n    }),\n    methods: {\n\n        async onSubmit() {\n\n            this.loading = true;\n\n            let self = this;\n\n            const formData = new FormData();\n\n            formData.append('Email', this.email);\n            formData.append('DisplayName', this.displayName);\n            formData.append('password', this.password);\n\n            await fetch('/registration', {\n                method: 'POST',\n                credentials: 'include',\n                body: formData\n            })\n                .then(async (response) => {\n\n                    self.loading = false;\n\n                    const json = await response.json();\n\n                    if (response.ok) {\n                        (0,_helpers_jwt_helpers__WEBPACK_IMPORTED_MODULE_0__.setToken)(json.token);\n                        window.location.replace(\"/\");\n                    }\n                    else {\n                        if (response.status === 400) {\n                            self.snackbarText = json.error;\n                            self.snackbar = true;\n                        }\n                    }\n\n                });\n\n        }\n    }\n});\n\n\n//# sourceURL=webpack://order-check/./vue-src/layouts/register.vue?./node_modules/vue-loader/lib/index.js??vue-loader-options");

/***/ }),

/***/ "./vue-src/layouts/register.vue?vue&type=script&lang=js&":
/*!***************************************************************!*\
  !*** ./vue-src/layouts/register.vue?vue&type=script&lang=js& ***!
  \***************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => __WEBPACK_DEFAULT_EXPORT__\n/* harmony export */ });\n/* harmony import */ var _node_modules_vue_loader_lib_index_js_vue_loader_options_register_vue_vue_type_script_lang_js___WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! -!../../node_modules/vue-loader/lib/index.js??vue-loader-options!./register.vue?vue&type=script&lang=js& */ \"./node_modules/vue-loader/lib/index.js??vue-loader-options!./vue-src/layouts/register.vue?vue&type=script&lang=js&\");\n /* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_vue_loader_lib_index_js_vue_loader_options_register_vue_vue_type_script_lang_js___WEBPACK_IMPORTED_MODULE_0__.default); \n\n//# sourceURL=webpack://order-check/./vue-src/layouts/register.vue?");

/***/ }),

/***/ "./vue-src/layouts/register.vue?vue&type=template&id=6b107229&scoped=true&":
/*!*********************************************************************************!*\
  !*** ./vue-src/layouts/register.vue?vue&type=template&id=6b107229&scoped=true& ***!
  \*********************************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"render\": () => /* reexport safe */ _node_modules_vue_loader_lib_loaders_templateLoader_js_vue_loader_options_node_modules_vue_loader_lib_index_js_vue_loader_options_register_vue_vue_type_template_id_6b107229_scoped_true___WEBPACK_IMPORTED_MODULE_0__.render,\n/* harmony export */   \"staticRenderFns\": () => /* reexport safe */ _node_modules_vue_loader_lib_loaders_templateLoader_js_vue_loader_options_node_modules_vue_loader_lib_index_js_vue_loader_options_register_vue_vue_type_template_id_6b107229_scoped_true___WEBPACK_IMPORTED_MODULE_0__.staticRenderFns\n/* harmony export */ });\n/* harmony import */ var _node_modules_vue_loader_lib_loaders_templateLoader_js_vue_loader_options_node_modules_vue_loader_lib_index_js_vue_loader_options_register_vue_vue_type_template_id_6b107229_scoped_true___WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! -!../../node_modules/vue-loader/lib/loaders/templateLoader.js??vue-loader-options!../../node_modules/vue-loader/lib/index.js??vue-loader-options!./register.vue?vue&type=template&id=6b107229&scoped=true& */ \"./node_modules/vue-loader/lib/loaders/templateLoader.js??vue-loader-options!./node_modules/vue-loader/lib/index.js??vue-loader-options!./vue-src/layouts/register.vue?vue&type=template&id=6b107229&scoped=true&\");\n\n\n//# sourceURL=webpack://order-check/./vue-src/layouts/register.vue?");

/***/ }),

/***/ "./node_modules/vue-loader/lib/loaders/templateLoader.js??vue-loader-options!./node_modules/vue-loader/lib/index.js??vue-loader-options!./vue-src/layouts/register.vue?vue&type=template&id=6b107229&scoped=true&":
/*!************************************************************************************************************************************************************************************************************************!*\
  !*** ./node_modules/vue-loader/lib/loaders/templateLoader.js??vue-loader-options!./node_modules/vue-loader/lib/index.js??vue-loader-options!./vue-src/layouts/register.vue?vue&type=template&id=6b107229&scoped=true& ***!
  \************************************************************************************************************************************************************************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"render\": () => /* binding */ render,\n/* harmony export */   \"staticRenderFns\": () => /* binding */ staticRenderFns\n/* harmony export */ });\nvar render = function() {\n  var _vm = this\n  var _h = _vm.$createElement\n  var _c = _vm._self._c || _h\n  return _c(\n    \"v-app\",\n    { staticStyle: { padding: \"10px\" }, attrs: { id: \"inspire\" } },\n    [\n      _c(\n        \"v-form\",\n        {\n          ref: \"registerForm\",\n          attrs: { action: \"registration\", method: \"POST\" },\n          model: {\n            value: _vm.valid,\n            callback: function($$v) {\n              _vm.valid = $$v\n            },\n            expression: \"valid\"\n          }\n        },\n        [\n          _c(\n            \"v-card\",\n            {\n              staticClass: \"mx-auto\",\n              staticStyle: { \"margin-top\": \"20px\" },\n              attrs: { \"max-width\": \"500\" }\n            },\n            [\n              _c(\"v-card-title\", [\n                _c(\"span\", { staticClass: \"title font-weight-light\" }, [\n                  _vm._v(\"Регистрация\")\n                ])\n              ]),\n              _vm._v(\" \"),\n              _c(\n                \"v-card-text\",\n                [\n                  _c(\"v-text-field\", {\n                    attrs: {\n                      label: \"Email\",\n                      name: \"Email\",\n                      rules: [_vm.rules.required, _vm.rules.email],\n                      required: \"\"\n                    },\n                    model: {\n                      value: _vm.email,\n                      callback: function($$v) {\n                        _vm.email = $$v\n                      },\n                      expression: \"email\"\n                    }\n                  }),\n                  _vm._v(\" \"),\n                  _c(\"v-text-field\", {\n                    attrs: {\n                      label: \"Ваше имя\",\n                      name: \"DisplayName\",\n                      rules: [_vm.rules.required],\n                      required: \"\"\n                    },\n                    model: {\n                      value: _vm.displayName,\n                      callback: function($$v) {\n                        _vm.displayName = $$v\n                      },\n                      expression: \"displayName\"\n                    }\n                  }),\n                  _vm._v(\" \"),\n                  _c(\"v-text-field\", {\n                    attrs: {\n                      \"append-icon\": _vm.showPassword\n                        ? \"mdi-eye\"\n                        : \"mdi-eye-off\",\n                      rules: [_vm.rules.required, _vm.rules.password],\n                      type: _vm.showPassword ? \"text\" : \"password\",\n                      label: \"Пароль\",\n                      counter: \"\",\n                      required: \"\",\n                      name: \"password\"\n                    },\n                    on: {\n                      \"click:append\": function($event) {\n                        _vm.showPassword = !_vm.showPassword\n                      }\n                    },\n                    model: {\n                      value: _vm.password,\n                      callback: function($$v) {\n                        _vm.password = $$v\n                      },\n                      expression: \"password\"\n                    }\n                  })\n                ],\n                1\n              ),\n              _vm._v(\" \"),\n              _c(\n                \"v-card-actions\",\n                [\n                  _c(\n                    \"v-btn\",\n                    {\n                      attrs: {\n                        block: \"\",\n                        disabled: !_vm.valid,\n                        color: \"success\"\n                      },\n                      on: { click: _vm.onSubmit }\n                    },\n                    [\n                      _vm._v(\n                        \"\\n                    отправить\\n                \"\n                      )\n                    ]\n                  )\n                ],\n                1\n              )\n            ],\n            1\n          ),\n          _vm._v(\" \"),\n          _c(\n            \"v-snackbar\",\n            {\n              attrs: { vertical: \"\", timeout: _vm.timeout },\n              scopedSlots: _vm._u([\n                {\n                  key: \"action\",\n                  fn: function(ref) {\n                    var attrs = ref.attrs\n                    return [\n                      _c(\n                        \"v-btn\",\n                        _vm._b(\n                          {\n                            attrs: { color: \"red\", text: \"\" },\n                            on: {\n                              click: function($event) {\n                                _vm.snackbar = false\n                              }\n                            }\n                          },\n                          \"v-btn\",\n                          attrs,\n                          false\n                        ),\n                        [\n                          _vm._v(\n                            \"\\n                    Закрыть\\n                \"\n                          )\n                        ]\n                      )\n                    ]\n                  }\n                }\n              ]),\n              model: {\n                value: _vm.snackbar,\n                callback: function($$v) {\n                  _vm.snackbar = $$v\n                },\n                expression: \"snackbar\"\n              }\n            },\n            [\n              _vm._v(\n                \"\\n            \" + _vm._s(_vm.snackbarText) + \"\\n\\n            \"\n              )\n            ]\n          )\n        ],\n        1\n      )\n    ],\n    1\n  )\n}\nvar staticRenderFns = []\nrender._withStripped = true\n\n\n\n//# sourceURL=webpack://order-check/./vue-src/layouts/register.vue?./node_modules/vue-loader/lib/loaders/templateLoader.js??vue-loader-options!./node_modules/vue-loader/lib/index.js??vue-loader-options");

/***/ }),

/***/ "./node_modules/vue-loader/lib/runtime/componentNormalizer.js":
/*!********************************************************************!*\
  !*** ./node_modules/vue-loader/lib/runtime/componentNormalizer.js ***!
  \********************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => /* binding */ normalizeComponent\n/* harmony export */ });\n/* globals __VUE_SSR_CONTEXT__ */\n\n// IMPORTANT: Do NOT use ES2015 features in this file (except for modules).\n// This module is a runtime utility for cleaner component module output and will\n// be included in the final webpack user bundle.\n\nfunction normalizeComponent (\n  scriptExports,\n  render,\n  staticRenderFns,\n  functionalTemplate,\n  injectStyles,\n  scopeId,\n  moduleIdentifier, /* server only */\n  shadowMode /* vue-cli only */\n) {\n  // Vue.extend constructor export interop\n  var options = typeof scriptExports === 'function'\n    ? scriptExports.options\n    : scriptExports\n\n  // render functions\n  if (render) {\n    options.render = render\n    options.staticRenderFns = staticRenderFns\n    options._compiled = true\n  }\n\n  // functional template\n  if (functionalTemplate) {\n    options.functional = true\n  }\n\n  // scopedId\n  if (scopeId) {\n    options._scopeId = 'data-v-' + scopeId\n  }\n\n  var hook\n  if (moduleIdentifier) { // server build\n    hook = function (context) {\n      // 2.3 injection\n      context =\n        context || // cached call\n        (this.$vnode && this.$vnode.ssrContext) || // stateful\n        (this.parent && this.parent.$vnode && this.parent.$vnode.ssrContext) // functional\n      // 2.2 with runInNewContext: true\n      if (!context && typeof __VUE_SSR_CONTEXT__ !== 'undefined') {\n        context = __VUE_SSR_CONTEXT__\n      }\n      // inject component styles\n      if (injectStyles) {\n        injectStyles.call(this, context)\n      }\n      // register component module identifier for async chunk inferrence\n      if (context && context._registeredComponents) {\n        context._registeredComponents.add(moduleIdentifier)\n      }\n    }\n    // used by ssr in case component is cached and beforeCreate\n    // never gets called\n    options._ssrRegister = hook\n  } else if (injectStyles) {\n    hook = shadowMode\n      ? function () {\n        injectStyles.call(\n          this,\n          (options.functional ? this.parent : this).$root.$options.shadowRoot\n        )\n      }\n      : injectStyles\n  }\n\n  if (hook) {\n    if (options.functional) {\n      // for template-only hot-reload because in that case the render fn doesn't\n      // go through the normalizer\n      options._injectStyles = hook\n      // register for functional component in vue file\n      var originalRender = options.render\n      options.render = function renderWithStyleInjection (h, context) {\n        hook.call(context)\n        return originalRender(h, context)\n      }\n    } else {\n      // inject component registration as beforeCreate hook\n      var existing = options.beforeCreate\n      options.beforeCreate = existing\n        ? [].concat(existing, hook)\n        : [hook]\n    }\n  }\n\n  return {\n    exports: scriptExports,\n    options: options\n  }\n}\n\n\n//# sourceURL=webpack://order-check/./node_modules/vue-loader/lib/runtime/componentNormalizer.js?");

/***/ }),

/***/ "./vue-src/helpers/jwt-helpers.js":
/*!****************************************!*\
  !*** ./vue-src/helpers/jwt-helpers.js ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"setToken\": () => /* binding */ setToken,\n/* harmony export */   \"getToken\": () => /* binding */ getToken,\n/* harmony export */   \"deleteToken\": () => /* binding */ deleteToken\n/* harmony export */ });\n﻿function setToken(token) {\r\n    localStorage.setItem('token', token);\r\n}\r\n\r\nfunction getToken() {\r\n    let token = localStorage.getItem('token');\r\n    return token;\r\n}\r\n\r\nfunction deleteToken() {\r\n    localStorage.removeItem('token'); \r\n}\n\n//# sourceURL=webpack://order-check/./vue-src/helpers/jwt-helpers.js?");

/***/ }),

/***/ "./vue-src/register.js":
/*!*****************************!*\
  !*** ./vue-src/register.js ***!
  \*****************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _layouts_register_vue__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./layouts/register.vue */ \"./vue-src/layouts/register.vue\");\n﻿\r\n\r\nnew Vue({\r\n    vuetify: new Vuetify(),\r\n    render: h => h(_layouts_register_vue__WEBPACK_IMPORTED_MODULE_0__.default)\r\n}).$mount(\"#app\");\r\n\r\n\n\n//# sourceURL=webpack://order-check/./vue-src/register.js?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		if(__webpack_module_cache__[moduleId]) {
/******/ 			return __webpack_module_cache__[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => Object.prototype.hasOwnProperty.call(obj, prop)
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	// startup
/******/ 	// Load entry module
/******/ 	__webpack_require__("./vue-src/register.js");
/******/ 	// This entry module used 'exports' so it can't be inlined
/******/ })()
;