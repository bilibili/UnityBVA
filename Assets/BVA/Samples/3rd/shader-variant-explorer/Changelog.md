# Changelog
All notable changes to this package will be documented in this file.
The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/) and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [0.3.0] - 2022-01-04
- fixed: compilation warnings due to UIToolkit API changes
- fixed: experimental menu option is now internal
- feat: preprocessed code for all ShaderTypes/stages is now generated

## [0.3.0-pre] - 2021-11-23
- fixed: compiles on 2021.2
- added: support for 2020.3.16+ ShaderCompiler API
- changed: preprocessing now happens on demand only for selected variants (giant shaders can now be explored!)
- added: copy/paste shader variant combinations from the breadcrumb navigation

## [0.2.5] - 2021-08-16
- added: pass compilation API now supported on 2020.3.16f1+

## [0.2.4] - 2021-07-26
- fixed: compiles again on 2021.1

## [0.2.3] - 2021-07-12
- fixed: compiled variant data now also logs arrays and struct child fields correctly

## [0.2.2] - 2021-07-12
- fixed: re-enable keywords that are not in found preprocessor combinations
- fixed: remove incorrect context menu that cleared import data for shaders
- added: more info about compiled passes is logged
- added: button to open a temp file with shader pass code and compiled byte code

## [0.2.1-exp] - 2021-07-10
- fixed: keyword ordering issue in some cases
- fixed: if the last keyword in a combination started with _ Unity rendered it as menu item
- improved: the breadcrumb "+" button will now only show available next keywords
- improved: additional separator line in collapse view for file start markers 

## [0.2.0-exp] - 2021-07-09
- initial release