﻿const path = require("path");
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");

module.exports = env => {
    const isDevBuild = !(env && env.prod);
    const extractCSS = new ExtractTextPlugin({
        filename: "site.css",
        disable: isDevBuild
    });

    const clientBundleOutputDir = "./wwwroot/dist";
    const clientBundleConfig = {
        entry: {
            "main-client": "./ClientApp/boot-client.tsx"
        },
        stats: { modules: false },
        resolve: { extensions: [".js", ".jsx", ".ts", ".tsx"] },
        output: {
            filename: "[name].js",
            publicPath: "/dist/", // Webpack dev middleware, if enabled, handles requests for this URL prefix
            path: path.join(__dirname, clientBundleOutputDir)
        },
        module: {
            rules: [
                {
                    test: /\.tsx?$/,
                    include: /ClientApp/,
                    loaders: ["awesome-typescript-loader?silent=true"]
                },
                {
                    test: /\.(png|jpg|jpeg|gif)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                    use: [
                        {
                            loader: "file-loader?name=src/img/[name].[ext]"
                        }
                    ]
                },
                {
                    test: /\.(woff2?|ttf|svg|eot)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                    use: [
                        {
                            loader: "file-loader?name=fonts/[name].[ext]"
                        }
                    ]
                },
                {
                    test: /\.scss$(\?|$)/,
                    use: extractCSS.extract({
                        fallback: "style-loader",
                        use: ["css-loader", "sass-loader"]
                    })
                }
            ]
        },
        plugins: [
            // Plugins that apply in development and production builds
            extractCSS,
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require("./wwwroot/dist/vendor-manifest.json")
            })
        ].concat(
            isDevBuild
                ? [
                    // Plugins that apply in development builds only
                    new webpack.SourceMapDevToolPlugin({
                        filename: "[file].map", // Remove this line if you prefer inline source maps
                        moduleFilenameTemplate: path.relative(
                            clientBundleOutputDir,
                            "[resourcePath]"
                        ) // Point sourcemap entries to the original file locations on disk
                    })
                ]
                : [
                    // Plugins that apply in production builds only
                    new UglifyJsPlugin({
                        cache: true,
                        parallel: true
                    })
                ]
        )
    };

    return clientBundleConfig;
};
