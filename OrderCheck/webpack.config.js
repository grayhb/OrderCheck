"use strict";
const path = require('path');
const VueLoaderPlugin = require('vue-loader/lib/plugin')

module.exports = {

    mode: 'development',

    entry: {
        app: path.resolve(__dirname, './vue-src/app.js'),
        register: path.resolve(__dirname, './vue-src/register.js'),
        login: path.resolve(__dirname, './vue-src/login.js'),
    },

    output: {
        path: path.resolve(__dirname, './wwwroot/bundles/js'),
        filename: '[name].bundle.js',
    },

    module: {
        rules: [
            //{
            //    test: /\.js$/,
            //    exclude: /node_modules/,
            //    use: ['babel-loader'],
            //},
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    //'vue-style-loader',
                    'css-loader'
                ]
            }
        ],
    },

    plugins: [
        new VueLoaderPlugin()
    ],

    resolve: {
        alias: {
            vue: 'vue/dist/vue.js'
        },
        extensions: ['.js', '.vue']
    },

    optimization: {
        minimize: false
    },
}
