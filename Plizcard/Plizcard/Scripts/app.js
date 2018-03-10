var $html = $('HTML');

$html.removeClass('no-js');

// Detecting touch
if ('ontouchstart' in window) {
	$html.addClass('touch');
} else {
	$html.addClass('no-touch');
}
/*! Copyright (c) 2013 Brandon Aaron (http://brandon.aaron.sh)
 * Licensed under the MIT License (LICENSE.txt).
 *
 * Version: 3.1.12
 *
 * Requires: jQuery 1.2.2+
 */

(function (factory) {
    if ( typeof define === 'function' && define.amd ) {
        // AMD. Register as an anonymous module.
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // Node/CommonJS style for Browserify
        module.exports = factory;
    } else {
        // Browser globals
        factory(jQuery);
    }
}(function ($) {

    var toFix  = ['wheel', 'mousewheel', 'DOMMouseScroll', 'MozMousePixelScroll'],
        toBind = ( 'onwheel' in document || document.documentMode >= 9 ) ?
                    ['wheel'] : ['mousewheel', 'DomMouseScroll', 'MozMousePixelScroll'],
        slice  = Array.prototype.slice,
        nullLowestDeltaTimeout, lowestDelta;

    if ( $.event.fixHooks ) {
        for ( var i = toFix.length; i; ) {
            $.event.fixHooks[ toFix[--i] ] = $.event.mouseHooks;
        }
    }

    var special = $.event.special.mousewheel = {
        version: '3.1.12',

        setup: function() {
            if ( this.addEventListener ) {
                for ( var i = toBind.length; i; ) {
                    this.addEventListener( toBind[--i], handler, false );
                }
            } else {
                this.onmousewheel = handler;
            }
            // Store the line height and page height for this particular element
            $.data(this, 'mousewheel-line-height', special.getLineHeight(this));
            $.data(this, 'mousewheel-page-height', special.getPageHeight(this));
        },

        teardown: function() {
            if ( this.removeEventListener ) {
                for ( var i = toBind.length; i; ) {
                    this.removeEventListener( toBind[--i], handler, false );
                }
            } else {
                this.onmousewheel = null;
            }
            // Clean up the data we added to the element
            $.removeData(this, 'mousewheel-line-height');
            $.removeData(this, 'mousewheel-page-height');
        },

        getLineHeight: function(elem) {
            var $elem = $(elem),
                $parent = $elem['offsetParent' in $.fn ? 'offsetParent' : 'parent']();
            if (!$parent.length) {
                $parent = $('body');
            }
            return parseInt($parent.css('fontSize'), 10) || parseInt($elem.css('fontSize'), 10) || 16;
        },

        getPageHeight: function(elem) {
            return $(elem).height();
        },

        settings: {
            adjustOldDeltas: true, // see shouldAdjustOldDeltas() below
            normalizeOffset: true  // calls getBoundingClientRect for each event
        }
    };

    $.fn.extend({
        mousewheel: function(fn) {
            return fn ? this.bind('mousewheel', fn) : this.trigger('mousewheel');
        },

        unmousewheel: function(fn) {
            return this.unbind('mousewheel', fn);
        }
    });


    function handler(event) {
        var orgEvent   = event || window.event,
            args       = slice.call(arguments, 1),
            delta      = 0,
            deltaX     = 0,
            deltaY     = 0,
            absDelta   = 0,
            offsetX    = 0,
            offsetY    = 0;
        event = $.event.fix(orgEvent);
        event.type = 'mousewheel';

        // Old school scrollwheel delta
        if ( 'detail'      in orgEvent ) { deltaY = orgEvent.detail * -1;      }
        if ( 'wheelDelta'  in orgEvent ) { deltaY = orgEvent.wheelDelta;       }
        if ( 'wheelDeltaY' in orgEvent ) { deltaY = orgEvent.wheelDeltaY;      }
        if ( 'wheelDeltaX' in orgEvent ) { deltaX = orgEvent.wheelDeltaX * -1; }

        // Firefox < 17 horizontal scrolling related to DOMMouseScroll event
        if ( 'axis' in orgEvent && orgEvent.axis === orgEvent.HORIZONTAL_AXIS ) {
            deltaX = deltaY * -1;
            deltaY = 0;
        }

        // Set delta to be deltaY or deltaX if deltaY is 0 for backwards compatabilitiy
        delta = deltaY === 0 ? deltaX : deltaY;

        // New school wheel delta (wheel event)
        if ( 'deltaY' in orgEvent ) {
            deltaY = orgEvent.deltaY * -1;
            delta  = deltaY;
        }
        if ( 'deltaX' in orgEvent ) {
            deltaX = orgEvent.deltaX;
            if ( deltaY === 0 ) { delta  = deltaX * -1; }
        }

        // No change actually happened, no reason to go any further
        if ( deltaY === 0 && deltaX === 0 ) { return; }

        // Need to convert lines and pages to pixels if we aren't already in pixels
        // There are three delta modes:
        //   * deltaMode 0 is by pixels, nothing to do
        //   * deltaMode 1 is by lines
        //   * deltaMode 2 is by pages
        if ( orgEvent.deltaMode === 1 ) {
            var lineHeight = $.data(this, 'mousewheel-line-height');
            delta  *= lineHeight;
            deltaY *= lineHeight;
            deltaX *= lineHeight;
        } else if ( orgEvent.deltaMode === 2 ) {
            var pageHeight = $.data(this, 'mousewheel-page-height');
            delta  *= pageHeight;
            deltaY *= pageHeight;
            deltaX *= pageHeight;
        }

        // Store lowest absolute delta to normalize the delta values
        absDelta = Math.max( Math.abs(deltaY), Math.abs(deltaX) );

        if ( !lowestDelta || absDelta < lowestDelta ) {
            lowestDelta = absDelta;

            // Adjust older deltas if necessary
            if ( shouldAdjustOldDeltas(orgEvent, absDelta) ) {
                lowestDelta /= 40;
            }
        }

        // Adjust older deltas if necessary
        if ( shouldAdjustOldDeltas(orgEvent, absDelta) ) {
            // Divide all the things by 40!
            delta  /= 40;
            deltaX /= 40;
            deltaY /= 40;
        }

        // Get a whole, normalized value for the deltas
        delta  = Math[ delta  >= 1 ? 'floor' : 'ceil' ](delta  / lowestDelta);
        deltaX = Math[ deltaX >= 1 ? 'floor' : 'ceil' ](deltaX / lowestDelta);
        deltaY = Math[ deltaY >= 1 ? 'floor' : 'ceil' ](deltaY / lowestDelta);

        // Normalise offsetX and offsetY properties
        if ( special.settings.normalizeOffset && this.getBoundingClientRect ) {
            var boundingRect = this.getBoundingClientRect();
            offsetX = event.clientX - boundingRect.left;
            offsetY = event.clientY - boundingRect.top;
        }

        // Add information to the event object
        event.deltaX = deltaX;
        event.deltaY = deltaY;
        event.deltaFactor = lowestDelta;
        event.offsetX = offsetX;
        event.offsetY = offsetY;
        // Go ahead and set deltaMode to 0 since we converted to pixels
        // Although this is a little odd since we overwrite the deltaX/Y
        // properties with normalized deltas.
        event.deltaMode = 0;

        // Add event and delta to the front of the arguments
        args.unshift(event, delta, deltaX, deltaY);

        // Clearout lowestDelta after sometime to better
        // handle multiple device types that give different
        // a different lowestDelta
        // Ex: trackpad = 3 and mouse wheel = 120
        if (nullLowestDeltaTimeout) { clearTimeout(nullLowestDeltaTimeout); }
        nullLowestDeltaTimeout = setTimeout(nullLowestDelta, 200);

        return ($.event.dispatch || $.event.handle).apply(this, args);
    }

    function nullLowestDelta() {
        lowestDelta = null;
    }

    function shouldAdjustOldDeltas(orgEvent, absDelta) {
        // If this is an older event and the delta is divisable by 120,
        // then we are assuming that the browser is treating this as an
        // older mouse wheel event and that we should divide the deltas
        // by 40 to try and get a more usable deltaFactor.
        // Side note, this actually impacts the reported scroll distance
        // in older browsers and can cause scrolling to be slower than native.
        // Turn this off by setting $.event.special.mousewheel.settings.adjustOldDeltas to false.
        return special.settings.adjustOldDeltas && orgEvent.type === 'mousewheel' && absDelta % 120 === 0;
    }

}));
/*
 jQuery Masked Input Plugin
 Copyright (c) 2007 - 2015 Josh Bush (digitalbush.com)
 Licensed under the MIT license (http://digitalbush.com/projects/masked-input-plugin/#license)
 Version: 1.4.1
 */
!function(factory) {
	"function" == typeof define && define.amd ? define([ "jquery" ], factory) : factory("object" == typeof exports ? require("jquery") : jQuery);
}(function($) {
	var caretTimeoutId, ua = navigator.userAgent, iPhone = /iphone/i.test(ua), chrome = /chrome/i.test(ua), android = /android/i.test(ua);
	$.mask = {
		definitions: {
			"9": "[0-9]",
			a: "[A-Za-z]",
			"*": "[A-Za-z0-9]"
		},
		autoclear: !0,
		dataName: "rawMaskFn",
		placeholder: "_"
	}, $.fn.extend({
		caret: function(begin, end) {
			var range;
			if (0 !== this.length && !this.is(":hidden")) return "number" == typeof begin ? (end = "number" == typeof end ? end : begin,
				this.each(function() {
					this.setSelectionRange ? this.setSelectionRange(begin, end) : this.createTextRange && (range = this.createTextRange(),
						range.collapse(!0), range.moveEnd("character", end), range.moveStart("character", begin),
						range.select());
				})) : (this[0].setSelectionRange ? (begin = this[0].selectionStart, end = this[0].selectionEnd) : document.selection && document.selection.createRange && (range = document.selection.createRange(),
				begin = 0 - range.duplicate().moveStart("character", -1e5), end = begin + range.text.length),
			{
				begin: begin,
				end: end
			});
		},
		unmask: function() {
			return this.trigger("unmask");
		},
		mask: function(mask, settings) {
			var input, defs, tests, partialPosition, firstNonMaskPos, lastRequiredNonMaskPos, len, oldVal;
			if (!mask && this.length > 0) {
				input = $(this[0]);
				var fn = input.data($.mask.dataName);
				return fn ? fn() : void 0;
			}
			return settings = $.extend({
				autoclear: $.mask.autoclear,
				placeholder: $.mask.placeholder,
				completed: null
			}, settings), defs = $.mask.definitions, tests = [], partialPosition = len = mask.length,
				firstNonMaskPos = null, $.each(mask.split(""), function(i, c) {
				"?" == c ? (len--, partialPosition = i) : defs[c] ? (tests.push(new RegExp(defs[c])),
				null === firstNonMaskPos && (firstNonMaskPos = tests.length - 1), partialPosition > i && (lastRequiredNonMaskPos = tests.length - 1)) : tests.push(null);
			}), this.trigger("unmask").each(function() {
				function tryFireCompleted() {
					if (settings.completed) {
						for (var i = firstNonMaskPos; lastRequiredNonMaskPos >= i; i++) if (tests[i] && buffer[i] === getPlaceholder(i)) return;
						settings.completed.call(input);
					}
				}
				function getPlaceholder(i) {
					return settings.placeholder.charAt(i < settings.placeholder.length ? i : 0);
				}
				function seekNext(pos) {
					for (;++pos < len && !tests[pos]; ) ;
					return pos;
				}
				function seekPrev(pos) {
					for (;--pos >= 0 && !tests[pos]; ) ;
					return pos;
				}
				function shiftL(begin, end) {
					var i, j;
					if (!(0 > begin)) {
						for (i = begin, j = seekNext(end); len > i; i++) if (tests[i]) {
							if (!(len > j && tests[i].test(buffer[j]))) break;
							buffer[i] = buffer[j], buffer[j] = getPlaceholder(j), j = seekNext(j);
						}
						writeBuffer(), input.caret(Math.max(firstNonMaskPos, begin));
					}
				}
				function shiftR(pos) {
					var i, c, j, t;
					for (i = pos, c = getPlaceholder(pos); len > i; i++) if (tests[i]) {
						if (j = seekNext(i), t = buffer[i], buffer[i] = c, !(len > j && tests[j].test(t))) break;
						c = t;
					}
				}
				function androidInputEvent() {
					var curVal = input.val(), pos = input.caret();
					if (oldVal && oldVal.length && oldVal.length > curVal.length) {
						for (checkVal(!0); pos.begin > 0 && !tests[pos.begin - 1]; ) pos.begin--;
						if (0 === pos.begin) for (;pos.begin < firstNonMaskPos && !tests[pos.begin]; ) pos.begin++;
						input.caret(pos.begin, pos.begin);
					} else {
						for (checkVal(!0); pos.begin < len && !tests[pos.begin]; ) pos.begin++;
						input.caret(pos.begin, pos.begin);
					}
					tryFireCompleted();
				}
				function blurEvent() {
					checkVal(), input.val() != focusText && input.change();
				}
				function keydownEvent(e) {
					if (!input.prop("readonly")) {
						var pos, begin, end, k = e.which || e.keyCode;
						oldVal = input.val(), 8 === k || 46 === k || iPhone && 127 === k ? (pos = input.caret(),
							begin = pos.begin, end = pos.end, end - begin === 0 && (begin = 46 !== k ? seekPrev(begin) : end = seekNext(begin - 1),
							end = 46 === k ? seekNext(end) : end), clearBuffer(begin, end), shiftL(begin, end - 1),
							e.preventDefault()) : 13 === k ? blurEvent.call(this, e) : 27 === k && (input.val(focusText),
							input.caret(0, checkVal()), e.preventDefault());
					}
				}
				function keypressEvent(e) {
					if (!input.prop("readonly")) {
						var p, c, next, k = e.which || e.keyCode, pos = input.caret();
						if (!(e.ctrlKey || e.altKey || e.metaKey || 32 > k) && k && 13 !== k) {
							if (pos.end - pos.begin !== 0 && (clearBuffer(pos.begin, pos.end), shiftL(pos.begin, pos.end - 1)),
									p = seekNext(pos.begin - 1), len > p && (c = String.fromCharCode(k), tests[p].test(c))) {
								if (shiftR(p), buffer[p] = c, writeBuffer(), next = seekNext(p), android) {
									var proxy = function() {
										$.proxy($.fn.caret, input, next)();
									};
									setTimeout(proxy, 0);
								} else input.caret(next);
								pos.begin <= lastRequiredNonMaskPos && tryFireCompleted();
							}
							e.preventDefault();
						}
					}
				}
				function clearBuffer(start, end) {
					var i;
					for (i = start; end > i && len > i; i++) tests[i] && (buffer[i] = getPlaceholder(i));
				}
				function writeBuffer() {
					input.val(buffer.join(""));
				}
				function checkVal(allow) {
					var i, c, pos, test = input.val(), lastMatch = -1;
					for (i = 0, pos = 0; len > i; i++) if (tests[i]) {
						for (buffer[i] = getPlaceholder(i); pos++ < test.length; ) if (c = test.charAt(pos - 1),
								tests[i].test(c)) {
							buffer[i] = c, lastMatch = i;
							break;
						}
						if (pos > test.length) {
							clearBuffer(i + 1, len);
							break;
						}
					} else buffer[i] === test.charAt(pos) && pos++, partialPosition > i && (lastMatch = i);
					return allow ? writeBuffer() : partialPosition > lastMatch + 1 ? settings.autoclear || buffer.join("") === defaultBuffer ? (input.val() && input.val(""),
						clearBuffer(0, len)) : writeBuffer() : (writeBuffer(), input.val(input.val().substring(0, lastMatch + 1))),
						partialPosition ? i : firstNonMaskPos;
				}
				var input = $(this), buffer = $.map(mask.split(""), function(c, i) {
					return "?" != c ? defs[c] ? getPlaceholder(i) : c : void 0;
				}), defaultBuffer = buffer.join(""), focusText = input.val();
				input.data($.mask.dataName, function() {
					return $.map(buffer, function(c, i) {
						return tests[i] && c != getPlaceholder(i) ? c : null;
					}).join("");
				}), input.one("unmask", function() {
					input.off(".mask").removeData($.mask.dataName);
				}).on("focus.mask", function() {
					if (!input.prop("readonly")) {
						clearTimeout(caretTimeoutId);
						var pos;
						focusText = input.val(), pos = checkVal(), caretTimeoutId = setTimeout(function() {
							input.get(0) === document.activeElement && (writeBuffer(), pos == mask.replace("?", "").length ? input.caret(0, pos) : input.caret(pos));
						}, 10);
					}
				}).on("blur.mask", blurEvent).on("keydown.mask", keydownEvent).on("keypress.mask", keypressEvent).on("input.mask paste.mask", function() {
					input.prop("readonly") || setTimeout(function() {
						var pos = checkVal(!0);
						input.caret(pos), tryFireCompleted();
					}, 0);
				}), chrome && android && input.off("input.mask").on("input.mask", androidInputEvent),
					checkVal();
			});
		}
	});
});
/*!
 * iCheck v2.0.0 rc1, http://git.io/arlzeA
 * =======================================
 * Cross-platform checkboxes and radio buttons customization
 *
 * (c) Damir Sultanov - http://fronteed.com
 * MIT Licensed
 */

(function(win, doc, $) {

	// prevent multiple includes
	if (!win.ichecked) {
		win.ichecked = function() {
			$ = win.jQuery || win.Zepto;

			// default options
			var defaults = {

				// auto init on domready
				autoInit: true,

				// auto handle ajax loaded inputs
				autoAjax: false,

				// remove 300ms click delay on touch devices
				tap: true,

				// customization class names
				checkboxClass: 'icheckbox',
				radioClass: 'iradio',

				checkedClass: 'checked',
				disabledClass: 'disabled',
				indeterminateClass: 'indeterminate',

				hoverClass: 'hover',
				// focusClass: 'focus',
				// activeClass: 'active',

				// default callbacks
				callbacks: {
					ifCreated: false
				},

				// appended class names
				classes: {
					base: 'icheck',
					div: '#-item', // {base}-item
					area: '#-area-', // {base}-area-{value}
					input: '#-input', // {base}-input
					label: '#-label' // {base}-label
				}
			};

			// extend default options
			win.icheck = $.extend(defaults, win.icheck);

			// useragent sniffing
			var ua = win.navigator.userAgent;
			var ie = /MSIE [5-8]/.test(ua) || doc.documentMode < 9;
			var operaMini = /Opera Mini/.test(ua);

			// classes cache
			var baseClass = defaults.classes.base;
			var divClass = defaults.classes.div.replace('#', baseClass);
			var areaClass = defaults.classes.area.replace('#', baseClass);
			var nodeClass = defaults.classes.input.replace('#', baseClass);
			var labelClass = defaults.classes.label.replace('#', baseClass);

			// unset init classes
			delete defaults.classes;

			// default filter
			var filter = 'input[type=checkbox],input[type=radio]';

			// clickable areas container
			var areas = {};

			// hashes container
			var hashes = {};

			// hash recognizer
			var recognizer = new RegExp(baseClass + '\\[(.*?)\\]');

			// hash extractor
			var extract = function(className, matches, value) {
				if (!!className) {
					matches = recognizer.exec(className);

					if (matches && hashes[matches[1]]) {
						value = matches[1];
					}
				}

				return value;
			};

			// detect computed style support
			var computed = win.getComputedStyle;

			// detect pointer events support
			var isPointer = win.PointerEvent || win.MSPointerEvent;

			// detect touch events support
			var isTouch = 'ontouchend' in win;

			// detect mobile users
			var isMobile = /mobile|tablet|phone|ip(ad|od)|android|silk|webos/i.test(ua);

			// setup events
			var mouse = ['mouse', 'down', 'up', 'over', 'out']; // bubbling hover
			var pointer = win.PointerEvent ? ['pointer', mouse[1], mouse[2], mouse[3], mouse[4]] : ['MSPointer', 'Down', 'Up', 'Over', 'Out'];
			var touch = ['touch', 'start', 'end'];
			var noMouse = (isTouch && isMobile) || isPointer;

			// choose events
			var hoverStart = noMouse ? (isTouch ? touch[0] + touch[1] : pointer[0] + pointer[3]) : mouse[0] + mouse[3];
			var hoverEnd = noMouse ? (isTouch ? touch[0] + touch[2] : pointer[0] + pointer[4]) : mouse[0] + mouse[4];
			var tapStart = noMouse ? (isTouch ? false : pointer[0] + pointer[1]) : mouse[0] + mouse[1];
			var tapEnd = noMouse ? (isTouch ? false : pointer[0] + pointer[2]) : mouse[0] + mouse[2];
			var hover = !operaMini ? hoverStart + '.i ' + hoverEnd + '.i ' : '';
			var tap = !operaMini && tapStart ? tapStart + '.i ' + tapEnd + '.i' : '';

			// styles options
			var styleTag;
			var styleList;
			var styleArea = defaults.areaStyle !== false ? 'position:absolute;display:block;content:"";top:#;bottom:#;left:#;right:#;' : 0;
			var styleInput = 'position:absolute!;display:block!;outline:none!;' + (defaults.debug ? '' : 'opacity:0!;z-index:-99!;clip:rect(0 0 0 0)!;');

			// styles addition
			var style = function(rules, selector, area) {
				if (!styleTag) {

					// create container
					styleTag = doc.createElement('style');

					// append to header
					(doc.head || doc.getElementsByTagName('head')[0]).appendChild(styleTag);

					// webkit hack
					if (!win.createPopup) {
						styleTag.appendChild(doc.createTextNode(''));
					}

					styleList = styleTag.sheet || styleTag.styleSheet;
				}

				// choose selector
				if (!selector) {
					selector = 'div.' + (area ? areaClass + area + ':after' : divClass + ' input.' + nodeClass);
				}

				// replace shorthand rules
				rules = rules.replace(/!/g, ' !important');

				// append styles
				if (styleList.addRule) {
					styleList.addRule(selector, rules, 0);
				} else {
					styleList.insertRule(selector + '{' + rules + '}', 0);
				}
			};

			// append input's styles
			style(styleInput);

			// append styler's styles
			if ((isTouch && isMobile) || operaMini) {

				// force custor:pointer for mobile devices
				style('cursor:pointer!;', 'label.' + labelClass + ',div.' + divClass);
			}

			// append iframe's styles
			style('display:none!', 'iframe.icheck-frame'); // used to handle ajax-loaded inputs

			// class toggler
			var toggle = function(node, className, status, currentClass, updatedClass, addClass, removeClass) {
				currentClass = node.className;

				if (!!currentClass) {
					updatedClass = ' ' + currentClass + ' ';

					// add class
					if (status === 1) {
						addClass = className;

						// remove class
					} else if (status === 0) {
						removeClass = className;

						// add and remove class
					} else {
						addClass = className[0];
						removeClass = className[1];
					}

					// add class
					if (!!addClass && updatedClass.indexOf(' ' + addClass + ' ') < 0) {
						updatedClass += addClass + ' ';
					}

					// remove class
					if (!!removeClass && ~updatedClass.indexOf(' ' + removeClass + ' ')) {
						updatedClass = updatedClass.replace(' ' + removeClass + ' ', ' ');
					}

					// trim class
					updatedClass = updatedClass.replace(/^\s+|\s+$/g, '');

					// update class
					if (updatedClass !== currentClass) {
						node.className = updatedClass;
					}

					// return updated class
					return updatedClass;
				}
			};

			// traces remover
			var tidy = function(node, key, trigger, settings, className, parent) {
				if (hashes[key]) {
					settings = hashes[key];
					className = settings.className;
					parent = $(closest(node, 'div', className));

					// prevent overlapping
					if (parent.length) {

						// input
						$(node).removeClass(nodeClass + ' ' + className).attr('style', settings.style);

						// label
						$('label.' + settings.esc).removeClass(labelClass + ' ' + className);

						// parent
						$(parent).replaceWith($(node));

						// callback
						if (trigger) {
							callback(node, key, trigger);
						}
					}

					// unset current key
					hashes[key] = false;
				}
			};

			// nodes inspector
			var inspect = function(object, node, stack, direct, indirect) {
				stack = [];
				direct = object.length;

				// inspect object
				while (direct--) {
					node = object[direct];

					// direct input
					if (node.type) {

						// checkbox or radio button
						if (~filter.indexOf(node.type)) {
							stack.push(node);
						}

						// indirect input
					} else {
						node = $(node).find(filter);
						indirect = node.length;

						while (indirect--) {
							stack.push(node[indirect]);
						}
					}
				}

				return stack;
			};

			// parent searcher
			var closest = function(node, tag, className, parent) {
				while (node && node.nodeType !== 9) {
					node = node.parentNode;

					if (node && node.tagName == tag.toUpperCase() && ~node.className.indexOf(className)) {
						parent = node;
						break;
					}
				}

				return parent;
			};

			// callbacks farm
			var callback = function(node, key, name) {
				name = 'if' + name;

				// callbacks are allowed
				if (!!hashes[key].callbacks) {

					// indirect callback
					if (hashes[key].callbacks[name] !== false) {
						$(node).trigger(name);

						// direct callback
						if (typeof hashes[key].callbacks[name] == 'function') {
							hashes[key].callbacks[name](node, hashes[key]);
						}
					}
				}
			};

			// selection processor
			var process = function(data, options, ajax, silent) {

				// get inputs
				var elements = inspect(data);
				var element = elements.length;

				// loop through inputs
				while (element--) {
					var node = elements[element];
					var nodeAttr = node.attributes;
					var nodeAttrCache = {};
					var nodeAttrLength = nodeAttr.length;
					var nodeAttrName;
					var nodeAttrValue;
					var nodeData = {};
					var nodeDataCache = {}; // merged data
					var nodeDataProperty;
					var nodeId = node.id;
					var nodeInherit;
					var nodeInheritItem;
					var nodeInheritLength;
					var nodeString = node.className;
					var nodeStyle;
					var nodeType = node.type;
					var queryData = $.cache ? $.cache[node[$.expando]] : 0; // cached data
					var settings;
					var key = extract(nodeString);
					var keyClass;
					var handle;
					var styler;
					var stylerClass = '';
					var stylerStyle;
					var area = false;
					var label;
					var labelDirect;
					var labelIndirect;
					var labelKey;
					var labelString;
					var labels = [];
					var labelsLength;
					var fastClass = win.FastClick ? ' needsclick' : '';

					// parse options from HTML attributes
					while (nodeAttrLength--) {
						nodeAttrName = nodeAttr[nodeAttrLength].name;
						nodeAttrValue = nodeAttr[nodeAttrLength].value;

						if (~nodeAttrName.indexOf('data-')) {
							nodeData[nodeAttrName.substr(5)] = nodeAttrValue;
						}

						if (nodeAttrName == 'style') {
							nodeStyle = nodeAttrValue;
						}

						nodeAttrCache[nodeAttrName] = nodeAttrValue;
					}

					// parse options from jQuery or Zepto cache
					if (queryData && queryData.data) {
						nodeData = $.extend(nodeData, queryData.data);
					}

					// parse merged options
					for (nodeDataProperty in nodeData) {
						nodeAttrValue = nodeData[nodeDataProperty];

						if (nodeAttrValue == 'true' || nodeAttrValue == 'false') {
							nodeAttrValue = nodeAttrValue == 'true';
						}

						nodeDataCache[nodeDataProperty.replace(/checkbox|radio|class|id|label/g, function(string, position) {
							return position === 0 ? string : string.charAt(0).toUpperCase() + string.slice(1);
						})] = nodeAttrValue;
					}

					// merge options
					settings = $.extend({}, defaults, win.icheck, nodeDataCache, options);

					// input type filter
					handle = settings.handle;

					if (handle !== 'checkbox' && handle !== 'radio') {
						handle = filter;
					}

					// prevent unwanted init
					if (settings.init !== false && ~handle.indexOf(nodeType)) {

						// tidy before processing
						if (key) {
							tidy(node, key);
						}

						// generate random key
						while(!hashes[key]) {
							key = Math.random().toString(36).substr(2, 5); // 5 symbols

							if (!hashes[key]) {
								keyClass = baseClass + '[' + key + ']';
								break;
							}
						}

						// prevent unwanted duplicates
						delete settings.autoInit;
						delete settings.autoAjax;

						// save settings
						settings.style = nodeStyle || '';
						settings.className = keyClass;
						settings.esc = keyClass.replace(/(\[|\])/g, '\\$1');
						hashes[key] = settings;

						// find direct label
						labelDirect = closest(node, 'label', '');

						if (labelDirect) {

							// normalize "for" attribute
							if (!!!labelDirect.htmlFor && !!nodeId) {
								labelDirect.htmlFor = nodeId;
							}

							labels.push(labelDirect);
						}

						// find indirect label
						if (!!nodeId) {
							labelIndirect = $('label[for="' + nodeId + '"]');

							// merge labels
							while (labelIndirect.length--) {
								label = labelIndirect[labelIndirect.length];

								if (label !== labelDirect) {
									labels.push(label);
								}
							}
						}

						// loop through labels
						labelsLength = labels.length;

						while (labelsLength--) {
							label = labels[labelsLength];
							labelString = label.className;
							labelKey = extract(labelString);

							// remove previous key
							if (labelKey) {
								labelString = toggle(label, baseClass + '[' + labelKey + ']', 0);
							} else {
								labelString = (!!labelString ? labelString + ' ' : '') + labelClass;
							}

							// update label's class
							label.className = labelString + ' ' + keyClass + fastClass;
						}

						// prepare styler
						styler = doc.createElement('div');

						// parse inherited options
						if (!!settings.inherit) {
							nodeInherit = settings.inherit.split(/\s*,\s*/);
							nodeInheritLength = nodeInherit.length;

							while (nodeInheritLength--) {
								nodeInheritItem = nodeInherit[nodeInheritLength];

								if (nodeAttrCache[nodeInheritItem] !== undefined) {
									if (nodeInheritItem == 'class') {
										stylerClass += nodeAttrCache[nodeInheritItem] + ' ';
									} else {
										styler.setAttribute(nodeInheritItem, nodeInheritItem == 'id' ? baseClass + '-' + nodeAttrCache[nodeInheritItem] : nodeAttrCache[nodeInheritItem]);
									}
								}
							}
						}

						// set input's type class
						stylerClass += settings[nodeType + 'Class'];

						// set styler's key
						stylerClass += ' ' + divClass + ' ' + keyClass;

						// append area styles
						if (settings.area && styleArea) {
							area = ('' + settings.area).replace(/%|px|em|\+|-/g, '') | 0;

							if (area) {

								// append area's styles
								if (!areas[area]) {
									style(styleArea.replace(/#/g, '-' + area + '%'), false, area);
									areas[area] = true;
								}

								stylerClass += ' ' + areaClass + area;
							}
						}

						// update styler's class
						styler.className = stylerClass + fastClass;

						// update node's class
						node.className = (!!nodeString ? nodeString + ' ' : '') + nodeClass + ' ' + keyClass;

						// replace node
						node.parentNode.replaceChild(styler, node);

						// append node
						styler.appendChild(node);

						// append additions
						if (!!settings.insert) {
							$(styler).append(settings.insert);
						}

						// set relative position
						if (area) {

							// get styler's position
							if (computed) {
								stylerStyle = computed(styler, null).getPropertyValue('position');
							} else {
								stylerStyle = styler.currentStyle.position;
							}

							// update styler's position
							if (stylerStyle == 'static') {
								styler.style.position = 'relative';
							}
						}

						// operate
						operate(node, styler, key, 'updated', true, false, ajax);
						hashes[key].done = true;

						// ifCreated callback
						if (!silent) {
							callback(node, key, 'Created');
						}
					}
				}
			};

			// operations center
			var operate = function(node, parent, key, method, silent, before, ajax) {
				var settings = hashes[key];
				var states = {};
				var changes = {};

				// current states
				states.checked = [node.checked, 'Checked', 'Unchecked'];

				if ((!before || ajax) && method !== 'click') {
					states.disabled = [node.disabled, 'Disabled', 'Enabled'];
					states.indeterminate = [node.getAttribute('indeterminate') == 'true' || !!node.indeterminate, 'Indeterminate', 'Determinate'];
				}

				// methods
				if (method == 'updated' || method == 'click') {
					changes.checked = before ? !states.checked[0] : states.checked[0];

					if ((!before || ajax) && method !== 'click') {
						changes.disabled = states.disabled[0];
						changes.indeterminate = states.indeterminate[0];
					}

				} else if (method == 'checked' || method == 'unchecked') {
					changes.checked = method == 'checked';

				} else if (method == 'disabled' || method == 'enabled') {
					changes.disabled = method == 'disabled';

				} else if (method == 'indeterminate' || method == 'determinate') {
					changes.indeterminate = method !== 'determinate';

					// "toggle" method
				} else {
					changes.checked = !states.checked[0];
				}

				// apply changes
				change(node, parent, states, changes, key, settings, method, silent, before, ajax);
			};

			// state changer
			var change = function(node, parent, states, changes, key, settings, method, silent, before, ajax, loop) {
				var type = node.type;
				var typeCapital = type == 'radio' ? 'Radio' : 'Checkbox';
				var property;
				var value;
				var classes;
				var inputClass;
				var label;
				var labelClass = 'LabelClass';
				var form;
				var radios;
				var radiosLength;
				var radio;
				var radioKey;
				var radioStates;
				var radioChanges;
				var changed;
				var toggled;

				// check parent
				if (!parent) {
					parent = closest(node, 'div', settings.className);
				}

				// continue if parent exists
				if (parent) {

					// detect changes
					for (property in changes) {
						value = changes[property];

						// update node's property
						if (states[property][0] !== value && method !== 'updated' && method !== 'click') {
							node[property] = value;
						}

						// update ajax attributes
						if (ajax) {
							if (value) {
								node.setAttribute(property, property);
							} else {
								node.removeAttribute(property);
							}
						}

						// update key's property
						if (settings[property] !== value) {
							settings[property] = value;
							changed = true;

							if (property == 'checked') {
								toggled = true;

								// find assigned radios
								if (!loop && value && (!!hashes[key].done || ajax) && type == 'radio' && !!node.name) {
									form = closest(node, 'form', '');
									radios = 'input[name="' + node.name + '"]';
									radios = form && !ajax ? $(form).find(radios) : $(radios);
									radiosLength = radios.length;

									while (radiosLength--) {
										radio = radios[radiosLength];
										radioKey = extract(radio.className);

										// toggle radios
										if (node !== radio && hashes[radioKey] && hashes[radioKey].checked) {
											radioStates = {checked: [true, 'Checked', 'Unchecked']};
											radioChanges = {checked: false};

											change(radio, false, radioStates, radioChanges, radioKey, hashes[radioKey], 'updated', silent, before, ajax, true);
										}
									}
								}
							}

							// cache classes
							classes = [
								settings[property + 'Class'], // 0, example: checkedClass
								settings[property + typeCapital + 'Class'], // 1, example: checkedCheckboxClass
								settings[states[property][1] + 'Class'], // 2, example: uncheckedClass
								settings[states[property][1] + typeCapital + 'Class'], // 3, example: uncheckedCheckboxClass
								settings[property + labelClass] // 4, example: checkedLabelClass
							];

							// value == false
							inputClass = [classes[3] || classes[2], classes[1] || classes[0]];

							// value == true
							if (value) {
								inputClass.reverse();
							}

							// update parent's class
							toggle(parent, inputClass);

							// update labels's class
							if (!!settings.mirror && !!classes[4]) {
								label = $('label.' + settings.esc);

								while (label.length--) {
									toggle(label[label.length], classes[4], value ? 1 : 0);
								}
							}

							// callback
							if (!silent || loop) {
								callback(node, key, states[property][value ? 1 : 2]); // ifChecked or ifUnchecked
							}
						}
					}

					// additional callbacks
					if (!silent || loop) {
						if (changed) {
							callback(node, key, 'Changed'); // ifChanged
						}

						if (toggled) {
							callback(node, key, 'Toggled'); // ifToggled
						}
					}

					// cursor addition
					if (!!settings.cursor && !isMobile) {

						// 'pointer' for enabled
						if (!settings.disabled && !settings.pointer) {
							parent.style.cursor = 'pointer';
							settings.pointer = true;

							// 'default' for disabled
						} else if (settings.disabled && settings.pointer) {
							parent.style.cursor = 'default';
							settings.pointer = false;
						}
					}

					// update settings
					hashes[key] = settings;
				}
			};

			// plugin definition
			$.fn.icheck = function(options, fire) {

				// detect methods
				if (/^(checked|unchecked|indeterminate|determinate|disabled|enabled|updated|toggle|destroy|data|styler)$/.test(options)) {
					var items = inspect(this);
					var itemsLength = items.length;

					// loop through inputs
					while (itemsLength--) {
						var item = items[itemsLength];
						var key = extract(item.className);

						if (key) {

							// 'data' method
							if (options == 'data') {
								return hashes[key];

								// 'styler' method
							} else if (options == 'styler') {
								return closest(item, 'div', hashes[key].className);

							} else {
								if (options == 'destroy') {
									tidy(item, key, 'Destroyed');
								} else {
									operate(item, false, key, options);
								}

								// callback
								if (typeof fire == 'function') {
									fire(item);
								}
							}
						}
					}

					// basic setup
				} else if (typeof options == 'object' || !options) {
					process(this, options || {});
				}

				// chain
				return this;
			};

			// cached last key
			var lastKey;

			// bind label and styler
			$(doc).on('click.i ' + hover + tap, 'label.' + labelClass + ',div.' + divClass, function(event) {
				var self = this;
				var key = extract(self.className);

				if (key) {
					var emitter = event.type;
					var settings = hashes[key];
					var className = settings.esc; // escaped class name
					var div = self.tagName == 'DIV';
					var input;
					var target;
					var partner;
					var activate;
					var states = [
						['label', settings.activeLabelClass, settings.hoverLabelClass],
						['div', settings.activeClass, settings.hoverClass]
					];

					// reverse array
					if (div) {
						states.reverse();
					}

					// active state
					if (emitter == tapStart || emitter == tapEnd) {

						// toggle self's active class
						if (!!states[0][1]) {
							toggle(self, states[0][1], emitter == tapStart ? 1 : 0);
						}

						// toggle partner's active class
						if (!!settings.mirror && !!states[1][1]) {
							partner = $(states[1][0] + '.' + className);

							while (partner.length--) {
								toggle(partner[partner.length], states[1][1], emitter == tapStart ? 1 : 0);
							}
						}

						// fast click
						if (div && emitter == tapEnd && !!settings.tap && isMobile && isPointer && !operaMini) {
							activate = true;
						}

						// hover state
					} else if (emitter == hoverStart || emitter == hoverEnd) {

						// toggle self's hover class
						if (!!states[0][2]) {
							toggle(self, states[0][2], emitter == hoverStart ? 1 : 0);
						}

						// toggle partner's hover class
						if (!!settings.mirror && !!states[1][2]) {
							partner = $(states[1][0] + '.' + className);

							while (partner.length--) {
								toggle(partner[partner.length], states[1][2], emitter == hoverStart ? 1 : 0);
							}
						}

						// fast click
						if (div && emitter == hoverEnd && !!settings.tap && isMobile && isTouch && !operaMini) {
							activate = true;
						}

						// click
					} else if (div) {
						if (!(isMobile && (isTouch || isPointer)) || !!!settings.tap || operaMini) {
							activate = true;
						}
					}

					// trigger input's click
					if (activate) {

						// currentTarget hack
						setTimeout(function() {
							target = event.currentTarget || {};

							if (target.tagName !== 'LABEL') {
								if (!settings.change || (+new Date() - settings.change > 100)) {
									input = $(self).find('input.' + className).click();

									if (ie || operaMini) {
										input.change();
									}
								}
							}
						}, 2);
					}
				}

				// bind input
			}).on('click.i change.i focusin.i focusout.i keyup.i keydown.i', 'input.' + nodeClass, function(event) {
				var self = this;
				var key = extract(self.className);

				if (key) {
					var emitter = event.type;
					var settings = hashes[key];
					var className = settings.esc; // escaped class name
					var parent = emitter == 'click' ? false : closest(self, 'div', settings.className);
					var label;
					var states;

					// click
					if (emitter == 'click') {
						hashes[key].change = +new Date();

						// prevent event bubbling to parent
						event.stopPropagation();

						// change
					} else if (emitter == 'change') {

						if (parent && !self.disabled) {
							operate(self, parent, key, 'click'); // 'click' method
						}

						// focus state
					} else if (~emitter.indexOf('focus')) {
						states = [settings.focusClass, settings.focusLabelClass];

						// toggle parent's focus class
						if (!!states[0] && parent) {
							toggle(parent, states[0], emitter == 'focusin' ? 1 : 0);
						}

						// toggle label's focus class
						if (!!settings.mirror && !!states[1]) {
							label = $('label.' + className);

							while (label.length--) {
								toggle(label[label.length], states[1], emitter == 'focusin' ? 1 : 0);
							}
						}

						// keyup or keydown (event fired before state is changed, except Opera 9-12)
					} else if (parent && !self.disabled) {

						// keyup
						if (emitter == 'keyup') {

							// spacebar or arrow
							if (self.type == 'checkbox' && event.keyCode == 32 && settings.keydown || self.type == 'radio' && !self.checked) {
								operate(self, parent, key, 'click', false, true); // 'toggle' method
							}

							hashes[key].keydown = false;
							hashes[lastKey] && (hashes[lastKey].keydown = false);

							// keydown
						} else {
							lastKey = key;
							hashes[key].keydown = true;
						}
					}
				}

				// domready
			}).ready(function() {

				// auto init
				if (win.icheck.autoInit) {
					$('.' + baseClass).icheck();
				}

				// auto ajax
				if (win.jQuery) {

					// body selector cache
					var body = doc.body || doc.getElementsByTagName('body')[0];

					// apply converter
					$.ajaxSetup({
						converters: {
							'text html': function(data) {
								if (win.icheck.autoAjax && body) {
									var frame = doc.createElement('iframe'); // create container
									var frameData;

									// set attributes
									if (!ie) {
										frame.style = 'display:none';
									}

									frame.className = 'iframe.icheck-frame';
									frame.src ='about:blank';

									// append container to document
									body.appendChild(frame);

									// save container's content
									frameData = frame.contentDocument ? frame.contentDocument : frame.contentWindow.document;

									// append data to content
									frameData.open();
									frameData.write(data);
									frameData.close();

									// remove container from document
									body.removeChild(frame);

									// setup object
									frameData = $(frameData);

									// customize inputs
									process(frameData.find('.' + baseClass), {}, true);

									// extract HTML
									frameData = frameData[0];
									data = (frameData.body || frameData.getElementsByTagName('body')[0]).innerHTML;
									frameData = null; // prevent memory leaks
								}

								return data;
							}
						}
					});
				}
			});
		};

		// expose iCheck as an AMD module
		if (typeof define == 'function' && define.amd) {
			define('icheck', [win.jQuery ? 'jquery' : 'zepto'], win.ichecked);
		} else {
			win.ichecked();
		}
	}
}(window, document));
/**
 * sifter.js
 * Copyright (c) 2013 Brian Reavis & contributors
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at:
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 *
 * @author Brian Reavis <brian@thirdroute.com>
 */

(function(root, factory) {
	if (typeof define === 'function' && define.amd) {
		define('sifter', factory);
	} else if (typeof exports === 'object') {
		module.exports = factory();
	} else {
		root.Sifter = factory();
	}
}(this, function() {

	/**
	 * Textually searches arrays and hashes of objects
	 * by property (or multiple properties). Designed
	 * specifically for autocomplete.
	 *
	 * @constructor
	 * @param {array|object} items
	 * @param {object} items
	 */
	var Sifter = function(items, settings) {
		this.items = items;
		this.settings = settings || {diacritics: true};
	};

	/**
	 * Splits a search string into an array of individual
	 * regexps to be used to match results.
	 *
	 * @param {string} query
	 * @returns {array}
	 */
	Sifter.prototype.tokenize = function(query) {
		query = trim(String(query || '').toLowerCase());
		if (!query || !query.length) return [];

		var i, n, regex, letter;
		var tokens = [];
		var words = query.split(/ +/);

		for (i = 0, n = words.length; i < n; i++) {
			regex = escape_regex(words[i]);
			if (this.settings.diacritics) {
				for (letter in DIACRITICS) {
					if (DIACRITICS.hasOwnProperty(letter)) {
						regex = regex.replace(new RegExp(letter, 'g'), DIACRITICS[letter]);
					}
				}
			}
			tokens.push({
				string : words[i],
				regex  : new RegExp(regex, 'i')
			});
		}

		return tokens;
	};

	/**
	 * Iterates over arrays and hashes.
	 *
	 * ```
	 * this.iterator(this.items, function(item, id) {
	 *    // invoked for each item
	 * });
	 * ```
	 *
	 * @param {array|object} object
	 */
	Sifter.prototype.iterator = function(object, callback) {
		var iterator;
		if (is_array(object)) {
			iterator = Array.prototype.forEach || function(callback) {
					for (var i = 0, n = this.length; i < n; i++) {
						callback(this[i], i, this);
					}
				};
		} else {
			iterator = function(callback) {
				for (var key in this) {
					if (this.hasOwnProperty(key)) {
						callback(this[key], key, this);
					}
				}
			};
		}

		iterator.apply(object, [callback]);
	};

	/**
	 * Returns a function to be used to score individual results.
	 *
	 * Good matches will have a higher score than poor matches.
	 * If an item is not a match, 0 will be returned by the function.
	 *
	 * @param {object|string} search
	 * @param {object} options (optional)
	 * @returns {function}
	 */
	Sifter.prototype.getScoreFunction = function(search, options) {
		var self, fields, tokens, token_count;

		self        = this;
		search      = self.prepareSearch(search, options);
		tokens      = search.tokens;
		fields      = search.options.fields;
		token_count = tokens.length;

		/**
		 * Calculates how close of a match the
		 * given value is against a search token.
		 *
		 * @param {mixed} value
		 * @param {object} token
		 * @return {number}
		 */
		var scoreValue = function(value, token) {
			var score, pos;

			if (!value) return 0;
			value = String(value || '');
			pos = value.search(token.regex);
			if (pos === -1) return 0;
			score = token.string.length / value.length;
			if (pos === 0) score += 0.5;
			return score;
		};

		/**
		 * Calculates the score of an object
		 * against the search query.
		 *
		 * @param {object} token
		 * @param {object} data
		 * @return {number}
		 */
		var scoreObject = (function() {
			var field_count = fields.length;
			if (!field_count) {
				return function() { return 0; };
			}
			if (field_count === 1) {
				return function(token, data) {
					return scoreValue(data[fields[0]], token);
				};
			}
			return function(token, data) {
				for (var i = 0, sum = 0; i < field_count; i++) {
					sum += scoreValue(data[fields[i]], token);
				}
				return sum / field_count;
			};
		})();

		if (!token_count) {
			return function() { return 0; };
		}
		if (token_count === 1) {
			return function(data) {
				return scoreObject(tokens[0], data);
			};
		}

		if (search.options.conjunction === 'and') {
			return function(data) {
				var score;
				for (var i = 0, sum = 0; i < token_count; i++) {
					score = scoreObject(tokens[i], data);
					if (score <= 0) return 0;
					sum += score;
				}
				return sum / token_count;
			};
		} else {
			return function(data) {
				for (var i = 0, sum = 0; i < token_count; i++) {
					sum += scoreObject(tokens[i], data);
				}
				return sum / token_count;
			};
		}
	};

	/**
	 * Returns a function that can be used to compare two
	 * results, for sorting purposes. If no sorting should
	 * be performed, `null` will be returned.
	 *
	 * @param {string|object} search
	 * @param {object} options
	 * @return function(a,b)
	 */
	Sifter.prototype.getSortFunction = function(search, options) {
		var i, n, self, field, fields, fields_count, multiplier, multipliers, get_field, implicit_score, sort;

		self   = this;
		search = self.prepareSearch(search, options);
		sort   = (!search.query && options.sort_empty) || options.sort;

		/**
		 * Fetches the specified sort field value
		 * from a search result item.
		 *
		 * @param  {string} name
		 * @param  {object} result
		 * @return {mixed}
		 */
		get_field = function(name, result) {
			if (name === '$score') return result.score;
			return self.items[result.id][name];
		};

		// parse options
		fields = [];
		if (sort) {
			for (i = 0, n = sort.length; i < n; i++) {
				if (search.query || sort[i].field !== '$score') {
					fields.push(sort[i]);
				}
			}
		}

		// the "$score" field is implied to be the primary
		// sort field, unless it's manually specified
		if (search.query) {
			implicit_score = true;
			for (i = 0, n = fields.length; i < n; i++) {
				if (fields[i].field === '$score') {
					implicit_score = false;
					break;
				}
			}
			if (implicit_score) {
				fields.unshift({field: '$score', direction: 'desc'});
			}
		} else {
			for (i = 0, n = fields.length; i < n; i++) {
				if (fields[i].field === '$score') {
					fields.splice(i, 1);
					break;
				}
			}
		}

		multipliers = [];
		for (i = 0, n = fields.length; i < n; i++) {
			multipliers.push(fields[i].direction === 'desc' ? -1 : 1);
		}

		// build function
		fields_count = fields.length;
		if (!fields_count) {
			return null;
		} else if (fields_count === 1) {
			field = fields[0].field;
			multiplier = multipliers[0];
			return function(a, b) {
				return multiplier * cmp(
						get_field(field, a),
						get_field(field, b)
					);
			};
		} else {
			return function(a, b) {
				var i, result, a_value, b_value, field;
				for (i = 0; i < fields_count; i++) {
					field = fields[i].field;
					result = multipliers[i] * cmp(
							get_field(field, a),
							get_field(field, b)
						);
					if (result) return result;
				}
				return 0;
			};
		}
	};

	/**
	 * Parses a search query and returns an object
	 * with tokens and fields ready to be populated
	 * with results.
	 *
	 * @param {string} query
	 * @param {object} options
	 * @returns {object}
	 */
	Sifter.prototype.prepareSearch = function(query, options) {
		if (typeof query === 'object') return query;

		options = extend({}, options);

		var option_fields     = options.fields;
		var option_sort       = options.sort;
		var option_sort_empty = options.sort_empty;

		if (option_fields && !is_array(option_fields)) options.fields = [option_fields];
		if (option_sort && !is_array(option_sort)) options.sort = [option_sort];
		if (option_sort_empty && !is_array(option_sort_empty)) options.sort_empty = [option_sort_empty];

		return {
			options : options,
			query   : String(query || '').toLowerCase(),
			tokens  : this.tokenize(query),
			total   : 0,
			items   : []
		};
	};

	/**
	 * Searches through all items and returns a sorted array of matches.
	 *
	 * The `options` parameter can contain:
	 *
	 *   - fields {string|array}
	 *   - sort {array}
	 *   - score {function}
	 *   - filter {bool}
	 *   - limit {integer}
	 *
	 * Returns an object containing:
	 *
	 *   - options {object}
	 *   - query {string}
	 *   - tokens {array}
	 *   - total {int}
	 *   - items {array}
	 *
	 * @param {string} query
	 * @param {object} options
	 * @returns {object}
	 */
	Sifter.prototype.search = function(query, options) {
		var self = this, value, score, search, calculateScore;
		var fn_sort;
		var fn_score;

		search  = this.prepareSearch(query, options);
		options = search.options;
		query   = search.query;

		// generate result scoring function
		fn_score = options.score || self.getScoreFunction(search);

		// perform search and sort
		if (query.length) {
			self.iterator(self.items, function(item, id) {
				score = fn_score(item);
				if (options.filter === false || score > 0) {
					search.items.push({'score': score, 'id': id});
				}
			});
		} else {
			self.iterator(self.items, function(item, id) {
				search.items.push({'score': 1, 'id': id});
			});
		}

		fn_sort = self.getSortFunction(search, options);
		if (fn_sort) search.items.sort(fn_sort);

		// apply limits
		search.total = search.items.length;
		if (typeof options.limit === 'number') {
			search.items = search.items.slice(0, options.limit);
		}

		return search;
	};

	// utilities
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	var cmp = function(a, b) {
		if (typeof a === 'number' && typeof b === 'number') {
			return a > b ? 1 : (a < b ? -1 : 0);
		}
		a = asciifold(String(a || ''));
		b = asciifold(String(b || ''));
		if (a > b) return 1;
		if (b > a) return -1;
		return 0;
	};

	var extend = function(a, b) {
		var i, n, k, object;
		for (i = 1, n = arguments.length; i < n; i++) {
			object = arguments[i];
			if (!object) continue;
			for (k in object) {
				if (object.hasOwnProperty(k)) {
					a[k] = object[k];
				}
			}
		}
		return a;
	};

	var trim = function(str) {
		return (str + '').replace(/^\s+|\s+$|/g, '');
	};

	var escape_regex = function(str) {
		return (str + '').replace(/([.?*+^$[\]\\(){}|-])/g, '\\$1');
	};

	var is_array = Array.isArray || ($ && $.isArray) || function(object) {
			return Object.prototype.toString.call(object) === '[object Array]';
		};

	var DIACRITICS = {
		'a': '[aÀÁÂÃÄÅàáâãäåĀāąĄ]',
		'c': '[cÇçćĆčČ]',
		'd': '[dđĐďĎ]',
		'e': '[eÈÉÊËèéêëěĚĒēęĘ]',
		'i': '[iÌÍÎÏìíîïĪī]',
		'l': '[lłŁ]',
		'n': '[nÑñňŇńŃ]',
		'o': '[oÒÓÔÕÕÖØòóôõöøŌō]',
		'r': '[rřŘ]',
		's': '[sŠšśŚ]',
		't': '[tťŤ]',
		'u': '[uÙÚÛÜùúûüůŮŪū]',
		'y': '[yŸÿýÝ]',
		'z': '[zŽžżŻźŹ]'
	};

	var asciifold = (function() {
		var i, n, k, chunk;
		var foreignletters = '';
		var lookup = {};
		for (k in DIACRITICS) {
			if (DIACRITICS.hasOwnProperty(k)) {
				chunk = DIACRITICS[k].substring(2, DIACRITICS[k].length - 1);
				foreignletters += chunk;
				for (i = 0, n = chunk.length; i < n; i++) {
					lookup[chunk.charAt(i)] = k;
				}
			}
		}
		var regexp = new RegExp('[' +  foreignletters + ']', 'g');
		return function(str) {
			return str.replace(regexp, function(foreignletter) {
				return lookup[foreignletter];
			}).toLowerCase();
		};
	})();


	// export
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	return Sifter;
}));



/**
 * microplugin.js
 * Copyright (c) 2013 Brian Reavis & contributors
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at:
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 *
 * @author Brian Reavis <brian@thirdroute.com>
 */

(function(root, factory) {
	if (typeof define === 'function' && define.amd) {
		define('microplugin', factory);
	} else if (typeof exports === 'object') {
		module.exports = factory();
	} else {
		root.MicroPlugin = factory();
	}
}(this, function() {
	var MicroPlugin = {};

	MicroPlugin.mixin = function(Interface) {
		Interface.plugins = {};

		/**
		 * Initializes the listed plugins (with options).
		 * Acceptable formats:
		 *
		 * List (without options):
		 *   ['a', 'b', 'c']
		 *
		 * List (with options):
		 *   [{'name': 'a', options: {}}, {'name': 'b', options: {}}]
		 *
		 * Hash (with options):
		 *   {'a': { ... }, 'b': { ... }, 'c': { ... }}
		 *
		 * @param {mixed} plugins
		 */
		Interface.prototype.initializePlugins = function(plugins) {
			var i, n, key;
			var self  = this;
			var queue = [];

			self.plugins = {
				names     : [],
				settings  : {},
				requested : {},
				loaded    : {}
			};

			if (utils.isArray(plugins)) {
				for (i = 0, n = plugins.length; i < n; i++) {
					if (typeof plugins[i] === 'string') {
						queue.push(plugins[i]);
					} else {
						self.plugins.settings[plugins[i].name] = plugins[i].options;
						queue.push(plugins[i].name);
					}
				}
			} else if (plugins) {
				for (key in plugins) {
					if (plugins.hasOwnProperty(key)) {
						self.plugins.settings[key] = plugins[key];
						queue.push(key);
					}
				}
			}

			while (queue.length) {
				self.require(queue.shift());
			}
		};

		Interface.prototype.loadPlugin = function(name) {
			var self    = this;
			var plugins = self.plugins;
			var plugin  = Interface.plugins[name];

			if (!Interface.plugins.hasOwnProperty(name)) {
				throw new Error('Unable to find "' +  name + '" plugin');
			}

			plugins.requested[name] = true;
			plugins.loaded[name] = plugin.fn.apply(self, [self.plugins.settings[name] || {}]);
			plugins.names.push(name);
		};

		/**
		 * Initializes a plugin.
		 *
		 * @param {string} name
		 */
		Interface.prototype.require = function(name) {
			var self = this;
			var plugins = self.plugins;

			if (!self.plugins.loaded.hasOwnProperty(name)) {
				if (plugins.requested[name]) {
					throw new Error('Plugin has circular dependency ("' + name + '")');
				}
				self.loadPlugin(name);
			}

			return plugins.loaded[name];
		};

		/**
		 * Registers a plugin.
		 *
		 * @param {string} name
		 * @param {function} fn
		 */
		Interface.define = function(name, fn) {
			Interface.plugins[name] = {
				'name' : name,
				'fn'   : fn
			};
		};
	};

	var utils = {
		isArray: Array.isArray || function(vArg) {
			return Object.prototype.toString.call(vArg) === '[object Array]';
		}
	};

	return MicroPlugin;
}));

/**
 * selectize.js (v0.12.1)
 * Copyright (c) 2013–2015 Brian Reavis & contributors
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at:
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 *
 * @author Brian Reavis <brian@thirdroute.com>
 */

/*jshint curly:false */
/*jshint browser:true */

(function(root, factory) {
	if (typeof define === 'function' && define.amd) {
		define('selectize', ['jquery','sifter','microplugin'], factory);
	} else if (typeof exports === 'object') {
		module.exports = factory(require('jquery'), require('sifter'), require('microplugin'));
	} else {
		root.Selectize = factory(root.jQuery, root.Sifter, root.MicroPlugin);
	}
}(this, function($, Sifter, MicroPlugin) {
	'use strict';

	var highlight = function($element, pattern) {
		if (typeof pattern === 'string' && !pattern.length) return;
		var regex = (typeof pattern === 'string') ? new RegExp(pattern, 'i') : pattern;

		var highlight = function(node) {
			var skip = 0;
			if (node.nodeType === 3) {
				var pos = node.data.search(regex);
				if (pos >= 0 && node.data.length > 0) {
					var match = node.data.match(regex);
					var spannode = document.createElement('span');
					spannode.className = 'highlight';
					var middlebit = node.splitText(pos);
					var endbit = middlebit.splitText(match[0].length);
					var middleclone = middlebit.cloneNode(true);
					spannode.appendChild(middleclone);
					middlebit.parentNode.replaceChild(spannode, middlebit);
					skip = 1;
				}
			} else if (node.nodeType === 1 && node.childNodes && !/(script|style)/i.test(node.tagName)) {
				for (var i = 0; i < node.childNodes.length; ++i) {
					i += highlight(node.childNodes[i]);
				}
			}
			return skip;
		};

		return $element.each(function() {
			highlight(this);
		});
	};
	
	var MicroEvent = function() {};
	MicroEvent.prototype = {
		on: function(event, fct){
			this._events = this._events || {};
			this._events[event] = this._events[event] || [];
			this._events[event].push(fct);
		},
		off: function(event, fct){
			var n = arguments.length;
			if (n === 0) return delete this._events;
			if (n === 1) return delete this._events[event];

			this._events = this._events || {};
			if (event in this._events === false) return;
			this._events[event].splice(this._events[event].indexOf(fct), 1);
		},
		trigger: function(event /* , args... */){
			this._events = this._events || {};
			if (event in this._events === false) return;
			for (var i = 0; i < this._events[event].length; i++){
				this._events[event][i].apply(this, Array.prototype.slice.call(arguments, 1));
			}
		}
	};
	
	/**
	 * Mixin will delegate all MicroEvent.js function in the destination object.
	 *
	 * - MicroEvent.mixin(Foobar) will make Foobar able to use MicroEvent
	 *
	 * @param {object} the object which will support MicroEvent
	 */
	MicroEvent.mixin = function(destObject){
		var props = ['on', 'off', 'trigger'];
		for (var i = 0; i < props.length; i++){
			destObject.prototype[props[i]] = MicroEvent.prototype[props[i]];
		}
	};
	
	var IS_MAC        = /Mac/.test(navigator.userAgent);
	
	var KEY_A         = 65;
	var KEY_COMMA     = 188;
	var KEY_RETURN    = 13;
	var KEY_ESC       = 27;
	var KEY_LEFT      = 37;
	var KEY_UP        = 38;
	var KEY_P         = 80;
	var KEY_RIGHT     = 39;
	var KEY_DOWN      = 40;
	var KEY_N         = 78;
	var KEY_BACKSPACE = 8;
	var KEY_DELETE    = 46;
	var KEY_SHIFT     = 16;
	var KEY_CMD       = IS_MAC ? 91 : 17;
	var KEY_CTRL      = IS_MAC ? 18 : 17;
	var KEY_TAB       = 9;
	
	var TAG_SELECT    = 1;
	var TAG_INPUT     = 2;
	
	// for now, android support in general is too spotty to support validity
	var SUPPORTS_VALIDITY_API = !/android/i.test(window.navigator.userAgent) && !!document.createElement('form').validity;
	
	var isset = function(object) {
		return typeof object !== 'undefined';
	};
	
	/**
	 * Converts a scalar to its best string representation
	 * for hash keys and HTML attribute values.
	 *
	 * Transformations:
	 *   'str'     -> 'str'
	 *   null      -> ''
	 *   undefined -> ''
	 *   true      -> '1'
	 *   false     -> '0'
	 *   0         -> '0'
	 *   1         -> '1'
	 *
	 * @param {string} value
	 * @returns {string|null}
	 */
	var hash_key = function(value) {
		if (typeof value === 'undefined' || value === null) return null;
		if (typeof value === 'boolean') return value ? '1' : '0';
		return value + '';
	};
	
	/**
	 * Escapes a string for use within HTML.
	 *
	 * @param {string} str
	 * @returns {string}
	 */
	var escape_html = function(str) {
		return (str + '')
			.replace(/&/g, '&amp;')
			.replace(/</g, '&lt;')
			.replace(/>/g, '&gt;')
			.replace(/"/g, '&quot;');
	};
	
	/**
	 * Escapes "$" characters in replacement strings.
	 *
	 * @param {string} str
	 * @returns {string}
	 */
	var escape_replace = function(str) {
		return (str + '').replace(/\$/g, '$$$$');
	};
	
	var hook = {};
	
	/**
	 * Wraps `method` on `self` so that `fn`
	 * is invoked before the original method.
	 *
	 * @param {object} self
	 * @param {string} method
	 * @param {function} fn
	 */
	hook.before = function(self, method, fn) {
		var original = self[method];
		self[method] = function() {
			fn.apply(self, arguments);
			return original.apply(self, arguments);
		};
	};
	
	/**
	 * Wraps `method` on `self` so that `fn`
	 * is invoked after the original method.
	 *
	 * @param {object} self
	 * @param {string} method
	 * @param {function} fn
	 */
	hook.after = function(self, method, fn) {
		var original = self[method];
		self[method] = function() {
			var result = original.apply(self, arguments);
			fn.apply(self, arguments);
			return result;
		};
	};
	
	/**
	 * Wraps `fn` so that it can only be invoked once.
	 *
	 * @param {function} fn
	 * @returns {function}
	 */
	var once = function(fn) {
		var called = false;
		return function() {
			if (called) return;
			called = true;
			fn.apply(this, arguments);
		};
	};
	
	/**
	 * Wraps `fn` so that it can only be called once
	 * every `delay` milliseconds (invoked on the falling edge).
	 *
	 * @param {function} fn
	 * @param {int} delay
	 * @returns {function}
	 */
	var debounce = function(fn, delay) {
		var timeout;
		return function() {
			var self = this;
			var args = arguments;
			window.clearTimeout(timeout);
			timeout = window.setTimeout(function() {
				fn.apply(self, args);
			}, delay);
		};
	};
	
	/**
	 * Debounce all fired events types listed in `types`
	 * while executing the provided `fn`.
	 *
	 * @param {object} self
	 * @param {array} types
	 * @param {function} fn
	 */
	var debounce_events = function(self, types, fn) {
		var type;
		var trigger = self.trigger;
		var event_args = {};

		// override trigger method
		self.trigger = function() {
			var type = arguments[0];
			if (types.indexOf(type) !== -1) {
				event_args[type] = arguments;
			} else {
				return trigger.apply(self, arguments);
			}
		};

		// invoke provided function
		fn.apply(self, []);
		self.trigger = trigger;

		// trigger queued events
		for (type in event_args) {
			if (event_args.hasOwnProperty(type)) {
				trigger.apply(self, event_args[type]);
			}
		}
	};
	
	/**
	 * A workaround for http://bugs.jquery.com/ticket/6696
	 *
	 * @param {object} $parent - Parent element to listen on.
	 * @param {string} event - Event name.
	 * @param {string} selector - Descendant selector to filter by.
	 * @param {function} fn - Event handler.
	 */
	var watchChildEvent = function($parent, event, selector, fn) {
		$parent.on(event, selector, function(e) {
			var child = e.target;
			while (child && child.parentNode !== $parent[0]) {
				child = child.parentNode;
			}
			e.currentTarget = child;
			return fn.apply(this, [e]);
		});
	};
	
	/**
	 * Determines the current selection within a text input control.
	 * Returns an object containing:
	 *   - start
	 *   - length
	 *
	 * @param {object} input
	 * @returns {object}
	 */
	var getSelection = function(input) {
		var result = {};
		if ('selectionStart' in input) {
			result.start = input.selectionStart;
			result.length = input.selectionEnd - result.start;
		} else if (document.selection) {
			input.focus();
			var sel = document.selection.createRange();
			var selLen = document.selection.createRange().text.length;
			sel.moveStart('character', -input.value.length);
			result.start = sel.text.length - selLen;
			result.length = selLen;
		}
		return result;
	};
	
	/**
	 * Copies CSS properties from one element to another.
	 *
	 * @param {object} $from
	 * @param {object} $to
	 * @param {array} properties
	 */
	var transferStyles = function($from, $to, properties) {
		var i, n, styles = {};
		if (properties) {
			for (i = 0, n = properties.length; i < n; i++) {
				styles[properties[i]] = $from.css(properties[i]);
			}
		} else {
			styles = $from.css();
		}
		$to.css(styles);
	};
	
	/**
	 * Measures the width of a string within a
	 * parent element (in pixels).
	 *
	 * @param {string} str
	 * @param {object} $parent
	 * @returns {int}
	 */
	var measureString = function(str, $parent) {
		if (!str) {
			return 0;
		}

		var $test = $('<test>').css({
			position: 'absolute',
			top: -99999,
			left: -99999,
			width: 'auto',
			padding: 0,
			whiteSpace: 'pre'
		}).text(str).appendTo('body');

		transferStyles($parent, $test, [
			'letterSpacing',
			'fontSize',
			'fontFamily',
			'fontWeight',
			'textTransform'
		]);

		var width = $test.width();
		$test.remove();

		return width;
	};
	
	/**
	 * Sets up an input to grow horizontally as the user
	 * types. If the value is changed manually, you can
	 * trigger the "update" handler to resize:
	 *
	 * $input.trigger('update');
	 *
	 * @param {object} $input
	 */
	var autoGrow = function($input) {
		var currentWidth = null;

		var update = function(e, options) {
			var value, keyCode, printable, placeholder, width;
			var shift, character, selection;
			e = e || window.event || {};
			options = options || {};

			if (e.metaKey || e.altKey) return;
			if (!options.force && $input.data('grow') === false) return;

			value = $input.val();
			if (e.type && e.type.toLowerCase() === 'keydown') {
				keyCode = e.keyCode;
				printable = (
					(keyCode >= 97 && keyCode <= 122) || // a-z
					(keyCode >= 65 && keyCode <= 90)  || // A-Z
					(keyCode >= 48 && keyCode <= 57)  || // 0-9
					keyCode === 32 // space
				);

				if (keyCode === KEY_DELETE || keyCode === KEY_BACKSPACE) {
					selection = getSelection($input[0]);
					if (selection.length) {
						value = value.substring(0, selection.start) + value.substring(selection.start + selection.length);
					} else if (keyCode === KEY_BACKSPACE && selection.start) {
						value = value.substring(0, selection.start - 1) + value.substring(selection.start + 1);
					} else if (keyCode === KEY_DELETE && typeof selection.start !== 'undefined') {
						value = value.substring(0, selection.start) + value.substring(selection.start + 1);
					}
				} else if (printable) {
					shift = e.shiftKey;
					character = String.fromCharCode(e.keyCode);
					if (shift) character = character.toUpperCase();
					else character = character.toLowerCase();
					value += character;
				}
			}

			placeholder = $input.attr('placeholder');
			if (!value && placeholder) {
				value = placeholder;
			}

			width = measureString(value, $input) + 4;
			if (width !== currentWidth) {
				currentWidth = width;
				$input.width(width);
				$input.triggerHandler('resize');
			}
		};

		$input.on('keydown keyup update blur', update);
		update();
	};
	
	var Selectize = function($input, settings) {
		var key, i, n, dir, input, self = this;
		input = $input[0];
		input.selectize = self;

		// detect rtl environment
		var computedStyle = window.getComputedStyle && window.getComputedStyle(input, null);
		dir = computedStyle ? computedStyle.getPropertyValue('direction') : input.currentStyle && input.currentStyle.direction;
		dir = dir || $input.parents('[dir]:first').attr('dir') || '';

		// setup default state
		$.extend(self, {
			order            : 0,
			settings         : settings,
			$input           : $input,
			tabIndex         : $input.attr('tabindex') || '',
			tagType          : input.tagName.toLowerCase() === 'select' ? TAG_SELECT : TAG_INPUT,
			rtl              : /rtl/i.test(dir),

			eventNS          : '.selectize' + (++Selectize.count),
			highlightedValue : null,
			isOpen           : false,
			isDisabled       : false,
			isRequired       : $input.is('[required]'),
			isInvalid        : false,
			isLocked         : false,
			isFocused        : false,
			isInputHidden    : false,
			isSetup          : false,
			isShiftDown      : false,
			isCmdDown        : false,
			isCtrlDown       : false,
			ignoreFocus      : false,
			ignoreBlur       : false,
			ignoreHover      : false,
			hasOptions       : false,
			currentResults   : null,
			lastValue        : '',
			caretPos         : 0,
			loading          : 0,
			loadedSearches   : {},

			$activeOption    : null,
			$activeItems     : [],

			optgroups        : {},
			options          : {},
			userOptions      : {},
			items            : [],
			renderCache      : {},
			onSearchChange   : settings.loadThrottle === null ? self.onSearchChange : debounce(self.onSearchChange, settings.loadThrottle)
		});

		// search system
		self.sifter = new Sifter(this.options, {diacritics: settings.diacritics});

		// build options table
		if (self.settings.options) {
			for (i = 0, n = self.settings.options.length; i < n; i++) {
				self.registerOption(self.settings.options[i]);
			}
			delete self.settings.options;
		}

		// build optgroup table
		if (self.settings.optgroups) {
			for (i = 0, n = self.settings.optgroups.length; i < n; i++) {
				self.registerOptionGroup(self.settings.optgroups[i]);
			}
			delete self.settings.optgroups;
		}

		// option-dependent defaults
		self.settings.mode = self.settings.mode || (self.settings.maxItems === 1 ? 'single' : 'multi');
		if (typeof self.settings.hideSelected !== 'boolean') {
			self.settings.hideSelected = self.settings.mode === 'multi';
		}

		self.initializePlugins(self.settings.plugins);
		self.setupCallbacks();
		self.setupTemplates();
		self.setup();
	};
	
	// mixins
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	
	MicroEvent.mixin(Selectize);
	MicroPlugin.mixin(Selectize);
	
	// methods
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	
	$.extend(Selectize.prototype, {

		/**
		 * Creates all elements and sets up event bindings.
		 */
		setup: function() {
			var self      = this;
			var settings  = self.settings;
			var eventNS   = self.eventNS;
			var $window   = $(window);
			var $document = $(document);
			var $input    = self.$input;

			var $wrapper;
			var $control;
			var $control_input;
			var $dropdown;
			var $dropdown_content;
			var $dropdown_parent;
			var inputMode;
			var timeout_blur;
			var timeout_focus;
			var classes;
			var classes_plugins;

			inputMode         = self.settings.mode;
			classes           = $input.attr('class') || '';

			$wrapper          = $('<div>').addClass(settings.wrapperClass).addClass(classes).addClass(inputMode);
			$control          = $('<div>').addClass(settings.inputClass).addClass('items').appendTo($wrapper);
			$control_input    = $('<input type="text" autocomplete="off" />').appendTo($control).attr('tabindex', $input.is(':disabled') ? '-1' : self.tabIndex);
			$dropdown_parent  = $(settings.dropdownParent || $wrapper);
			$dropdown         = $('<div>').addClass(settings.dropdownClass).addClass(inputMode).hide().appendTo($dropdown_parent);
			$dropdown_content = $('<div>').addClass(settings.dropdownContentClass).appendTo($dropdown);

			if(self.settings.copyClassesToDropdown) {
				$dropdown.addClass(classes);
			}

			$wrapper.css({
				width: $input[0].style.width
			});

			if (self.plugins.names.length) {
				classes_plugins = 'plugin-' + self.plugins.names.join(' plugin-');
				$wrapper.addClass(classes_plugins);
				$dropdown.addClass(classes_plugins);
			}

			if ((settings.maxItems === null || settings.maxItems > 1) && self.tagType === TAG_SELECT) {
				$input.attr('multiple', 'multiple');
			}

			if (self.settings.placeholder) {
				$control_input.attr('placeholder', settings.placeholder);
			}

			// if splitOn was not passed in, construct it from the delimiter to allow pasting universally
			if (!self.settings.splitOn && self.settings.delimiter) {
				var delimiterEscaped = self.settings.delimiter.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
				self.settings.splitOn = new RegExp('\\s*' + delimiterEscaped + '+\\s*');
			}

			if ($input.attr('autocorrect')) {
				$control_input.attr('autocorrect', $input.attr('autocorrect'));
			}

			if ($input.attr('autocapitalize')) {
				$control_input.attr('autocapitalize', $input.attr('autocapitalize'));
			}

			self.$wrapper          = $wrapper;
			self.$control          = $control;
			self.$control_input    = $control_input;
			self.$dropdown         = $dropdown;
			self.$dropdown_content = $dropdown_content;

			$dropdown.on('mouseenter', '[data-selectable]', function() { return self.onOptionHover.apply(self, arguments); });
			$dropdown.on('mousedown click', '[data-selectable]', function() { return self.onOptionSelect.apply(self, arguments); });
			watchChildEvent($control, 'mousedown', '*:not(input)', function() { return self.onItemSelect.apply(self, arguments); });
			autoGrow($control_input);

			$control.on({
				mousedown : function() { return self.onMouseDown.apply(self, arguments); },
				click     : function() { return self.onClick.apply(self, arguments); }
			});

			$control_input.on({
				mousedown : function(e) { e.stopPropagation(); },
				keydown   : function() { return self.onKeyDown.apply(self, arguments); },
				keyup     : function() { return self.onKeyUp.apply(self, arguments); },
				keypress  : function() { return self.onKeyPress.apply(self, arguments); },
				resize    : function() { self.positionDropdown.apply(self, []); },
				blur      : function() { return self.onBlur.apply(self, arguments); },
				focus     : function() { self.ignoreBlur = false; return self.onFocus.apply(self, arguments); },
				paste     : function() { return self.onPaste.apply(self, arguments); }
			});

			$document.on('keydown' + eventNS, function(e) {
				self.isCmdDown = e[IS_MAC ? 'metaKey' : 'ctrlKey'];
				self.isCtrlDown = e[IS_MAC ? 'altKey' : 'ctrlKey'];
				self.isShiftDown = e.shiftKey;
			});

			$document.on('keyup' + eventNS, function(e) {
				if (e.keyCode === KEY_CTRL) self.isCtrlDown = false;
				if (e.keyCode === KEY_SHIFT) self.isShiftDown = false;
				if (e.keyCode === KEY_CMD) self.isCmdDown = false;
			});

			$document.on('mousedown' + eventNS, function(e) {
				if (self.isFocused) {
					// prevent events on the dropdown scrollbar from causing the control to blur
					if (e.target === self.$dropdown[0] || e.target.parentNode === self.$dropdown[0]) {
						return false;
					}
					// blur on click outside
					if (!self.$control.has(e.target).length && e.target !== self.$control[0]) {
						self.blur(e.target);
					}
				}
			});

			$window.on(['scroll' + eventNS, 'resize' + eventNS].join(' '), function() {
				if (self.isOpen) {
					self.positionDropdown.apply(self, arguments);
				}
			});
			$window.on('mousemove' + eventNS, function() {
				self.ignoreHover = false;
			});

			// store original children and tab index so that they can be
			// restored when the destroy() method is called.
			this.revertSettings = {
				$children : $input.children().detach(),
				tabindex  : $input.attr('tabindex')
			};

			$input.attr('tabindex', -1).hide().after(self.$wrapper);

			if ($.isArray(settings.items)) {
				self.setValue(settings.items);
				delete settings.items;
			}

			// feature detect for the validation API
			if (SUPPORTS_VALIDITY_API) {
				$input.on('invalid' + eventNS, function(e) {
					e.preventDefault();
					self.isInvalid = true;
					self.refreshState();
				});
			}

			self.updateOriginalInput();
			self.refreshItems();
			self.refreshState();
			self.updatePlaceholder();
			self.isSetup = true;

			if ($input.is(':disabled')) {
				self.disable();
			}

			self.on('change', this.onChange);

			$input.data('selectize', self);
			$input.addClass('selectized');
			self.trigger('initialize');

			// preload options
			if (settings.preload === true) {
				self.onSearchChange('');
			}

		},

		/**
		 * Sets up default rendering functions.
		 */
		setupTemplates: function() {
			var self = this;
			var field_label = self.settings.labelField;
			var field_optgroup = self.settings.optgroupLabelField;

			var templates = {
				'optgroup': function(data) {
					return '<div class="optgroup">' + data.html + '</div>';
				},
				'optgroup_header': function(data, escape) {
					return '<div class="optgroup-header">' + escape(data[field_optgroup]) + '</div>';
				},
				'option': function(data, escape) {
					return '<div class="option">' + escape(data[field_label]) + '</div>';
				},
				'item': function(data, escape) {
					return '<div class="item">' + escape(data[field_label]) + '</div>';
				},
				'option_create': function(data, escape) {
					return '<div class="create">Добавить <strong>' + escape(data.input) + '</strong>&hellip;</div>';
				}
			};

			self.settings.render = $.extend({}, templates, self.settings.render);
		},

		/**
		 * Maps fired events to callbacks provided
		 * in the settings used when creating the control.
		 */
		setupCallbacks: function() {
			var key, fn, callbacks = {
				'initialize'      : 'onInitialize',
				'change'          : 'onChange',
				'item_add'        : 'onItemAdd',
				'item_remove'     : 'onItemRemove',
				'clear'           : 'onClear',
				'option_add'      : 'onOptionAdd',
				'option_remove'   : 'onOptionRemove',
				'option_clear'    : 'onOptionClear',
				'optgroup_add'    : 'onOptionGroupAdd',
				'optgroup_remove' : 'onOptionGroupRemove',
				'optgroup_clear'  : 'onOptionGroupClear',
				'dropdown_open'   : 'onDropdownOpen',
				'dropdown_close'  : 'onDropdownClose',
				'type'            : 'onType',
				'load'            : 'onLoad',
				'focus'           : 'onFocus',
				'blur'            : 'onBlur'
			};

			for (key in callbacks) {
				if (callbacks.hasOwnProperty(key)) {
					fn = this.settings[callbacks[key]];
					if (fn) this.on(key, fn);
				}
			}
		},

		/**
		 * Triggered when the main control element
		 * has a click event.
		 *
		 * @param {object} e
		 * @return {boolean}
		 */
		onClick: function(e) {
			var self = this;

			// necessary for mobile webkit devices (manual focus triggering
			// is ignored unless invoked within a click event)
			if (!self.isFocused) {
				self.focus();
				e.preventDefault();
			}
		},

		/**
		 * Triggered when the main control element
		 * has a mouse down event.
		 *
		 * @param {object} e
		 * @return {boolean}
		 */
		onMouseDown: function(e) {
			var self = this;
			var defaultPrevented = e.isDefaultPrevented();
			var $target = $(e.target);

			if (self.isFocused) {
				// retain focus by preventing native handling. if the
				// event target is the input it should not be modified.
				// otherwise, text selection within the input won't work.
				if (e.target !== self.$control_input[0]) {
					if (self.settings.mode === 'single') {
						// toggle dropdown
						self.isOpen ? self.close() : self.open();
					} else if (!defaultPrevented) {
						self.setActiveItem(null);
					}
					return false;
				}
			} else {
				// give control focus
				if (!defaultPrevented) {
					window.setTimeout(function() {
						self.focus();
					}, 0);
				}
			}
		},

		/**
		 * Triggered when the value of the control has been changed.
		 * This should propagate the event to the original DOM
		 * input / select element.
		 */
		onChange: function() {
			this.$input.trigger('change');
		},

		/**
		 * Triggered on <input> paste.
		 *
		 * @param {object} e
		 * @returns {boolean}
		 */
		onPaste: function(e) {
			var self = this;
			if (self.isFull() || self.isInputHidden || self.isLocked) {
				e.preventDefault();
			} else {
				// If a regex or string is included, this will split the pasted
				// input and create Items for each separate value
				if (self.settings.splitOn) {
					setTimeout(function() {
						var splitInput = $.trim(self.$control_input.val() || '').split(self.settings.splitOn);
						for (var i = 0, n = splitInput.length; i < n; i++) {
							self.createItem(splitInput[i]);
						}
					}, 0);
				}
			}
		},

		/**
		 * Triggered on <input> keypress.
		 *
		 * @param {object} e
		 * @returns {boolean}
		 */
		onKeyPress: function(e) {
			if (this.isLocked) return e && e.preventDefault();
			var character = String.fromCharCode(e.keyCode || e.which);
			if (this.settings.create && this.settings.mode === 'multi' && character === this.settings.delimiter) {
				this.createItem();
				e.preventDefault();
				return false;
			}
		},

		/**
		 * Triggered on <input> keydown.
		 *
		 * @param {object} e
		 * @returns {boolean}
		 */
		onKeyDown: function(e) {
			var isInput = e.target === this.$control_input[0];
			var self = this;

			if (self.isLocked) {
				if (e.keyCode !== KEY_TAB) {
					e.preventDefault();
				}
				return;
			}

			switch (e.keyCode) {
				case KEY_A:
					if (self.isCmdDown) {
						self.selectAll();
						return;
					}
					break;
				case KEY_ESC:
					if (self.isOpen) {
						e.preventDefault();
						e.stopPropagation();
						self.close();
					}
					return;
				case KEY_N:
					if (!e.ctrlKey || e.altKey) break;
				case KEY_DOWN:
					if (!self.isOpen && self.hasOptions) {
						self.open();
					} else if (self.$activeOption) {
						self.ignoreHover = true;
						var $next = self.getAdjacentOption(self.$activeOption, 1);
						if ($next.length) self.setActiveOption($next, true, true);
					}
					e.preventDefault();
					return;
				case KEY_P:
					if (!e.ctrlKey || e.altKey) break;
				case KEY_UP:
					if (self.$activeOption) {
						self.ignoreHover = true;
						var $prev = self.getAdjacentOption(self.$activeOption, -1);
						if ($prev.length) self.setActiveOption($prev, true, true);
					}
					e.preventDefault();
					return;
				case KEY_RETURN:
					if (self.isOpen && self.$activeOption) {
						self.onOptionSelect({currentTarget: self.$activeOption});
						e.preventDefault();
					}
					return;
				case KEY_LEFT:
					self.advanceSelection(-1, e);
					return;
				case KEY_RIGHT:
					self.advanceSelection(1, e);
					return;
				case KEY_TAB:
					if (self.settings.selectOnTab && self.isOpen && self.$activeOption) {
						self.onOptionSelect({currentTarget: self.$activeOption});

						// Default behaviour is to jump to the next field, we only want this
						// if the current field doesn't accept any more entries
						if (!self.isFull()) {
							e.preventDefault();
						}
					}
					if (self.settings.create && self.createItem()) {
						e.preventDefault();
					}
					return;
				case KEY_BACKSPACE:
				case KEY_DELETE:
					self.deleteSelection(e);
					return;
			}

			if ((self.isFull() || self.isInputHidden) && !(IS_MAC ? e.metaKey : e.ctrlKey)) {
				e.preventDefault();
				return;
			}
		},

		/**
		 * Triggered on <input> keyup.
		 *
		 * @param {object} e
		 * @returns {boolean}
		 */
		onKeyUp: function(e) {
			var self = this;

			if (self.isLocked) return e && e.preventDefault();
			var value = self.$control_input.val() || '';
			if (self.lastValue !== value) {
				self.lastValue = value;
				self.onSearchChange(value);
				self.refreshOptions();
				self.trigger('type', value);
			}
		},

		/**
		 * Invokes the user-provide option provider / loader.
		 *
		 * Note: this function is debounced in the Selectize
		 * constructor (by `settings.loadDelay` milliseconds)
		 *
		 * @param {string} value
		 */
		onSearchChange: function(value) {
			var self = this;
			var fn = self.settings.load;
			if (!fn) return;
			if (self.loadedSearches.hasOwnProperty(value)) return;
			self.loadedSearches[value] = true;
			self.load(function(callback) {
				fn.apply(self, [value, callback]);
			});
		},

		/**
		 * Triggered on <input> focus.
		 *
		 * @param {object} e (optional)
		 * @returns {boolean}
		 */
		onFocus: function(e) {
			var self = this;
			var wasFocused = self.isFocused;

			if (self.isDisabled) {
				self.blur();
				e && e.preventDefault();
				return false;
			}

			if (self.ignoreFocus) return;
			self.isFocused = true;
			if (self.settings.preload === 'focus') self.onSearchChange('');

			if (!wasFocused) self.trigger('focus');

			if (!self.$activeItems.length) {
				self.showInput();
				self.setActiveItem(null);
				self.refreshOptions(!!self.settings.openOnFocus);
			}

			self.refreshState();
		},

		/**
		 * Triggered on <input> blur.
		 *
		 * @param {object} e
		 * @param {Element} dest
		 */
		onBlur: function(e, dest) {
			var self = this;
			if (!self.isFocused) return;
			self.isFocused = false;

			if (self.ignoreFocus) {
				return;
			} else if (!self.ignoreBlur && document.activeElement === self.$dropdown_content[0]) {
				// necessary to prevent IE closing the dropdown when the scrollbar is clicked
				self.ignoreBlur = true;
				self.onFocus(e);
				return;
			}

			var deactivate = function() {
				self.close();
				self.setTextboxValue('');
				self.setActiveItem(null);
				self.setActiveOption(null);
				self.setCaret(self.items.length);
				self.refreshState();

				// IE11 bug: element still marked as active
				(dest || document.body).focus();

				self.ignoreFocus = false;
				self.trigger('blur');
			};

			self.ignoreFocus = true;
			if (self.settings.create && self.settings.createOnBlur) {
				self.createItem(null, false, deactivate);
			} else {
				deactivate();
			}
		},

		/**
		 * Triggered when the user rolls over
		 * an option in the autocomplete dropdown menu.
		 *
		 * @param {object} e
		 * @returns {boolean}
		 */
		onOptionHover: function(e) {
			if (this.ignoreHover) return;
			this.setActiveOption(e.currentTarget, false);
		},

		/**
		 * Triggered when the user clicks on an option
		 * in the autocomplete dropdown menu.
		 *
		 * @param {object} e
		 * @returns {boolean}
		 */
		onOptionSelect: function(e) {
			var value, $target, $option, self = this;

			if (e.preventDefault) {
				e.preventDefault();
				e.stopPropagation();
			}

			$target = $(e.currentTarget);
			if ($target.hasClass('create')) {
				self.createItem(null, function() {
					if (self.settings.closeAfterSelect) {
						self.close();
					}
				});
			} else {
				value = $target.attr('data-value');
				if (typeof value !== 'undefined') {
					self.lastQuery = null;
					self.setTextboxValue('');
					self.addItem(value);
					if (self.settings.closeAfterSelect) {
						self.close();
					} else if (!self.settings.hideSelected && e.type && /mouse/.test(e.type)) {
						self.setActiveOption(self.getOption(value));
					}
				}
			}
		},

		/**
		 * Triggered when the user clicks on an item
		 * that has been selected.
		 *
		 * @param {object} e
		 * @returns {boolean}
		 */
		onItemSelect: function(e) {
			var self = this;

			if (self.isLocked) return;
			if (self.settings.mode === 'multi') {
				e.preventDefault();
				self.setActiveItem(e.currentTarget, e);
			}
		},

		/**
		 * Invokes the provided method that provides
		 * results to a callback---which are then added
		 * as options to the control.
		 *
		 * @param {function} fn
		 */
		load: function(fn) {
			var self = this;
			var $wrapper = self.$wrapper.addClass(self.settings.loadingClass);

			self.loading++;
			fn.apply(self, [function(results) {
				self.loading = Math.max(self.loading - 1, 0);
				if (results && results.length) {
					self.addOption(results);
					self.refreshOptions(self.isFocused && !self.isInputHidden);
				}
				if (!self.loading) {
					$wrapper.removeClass(self.settings.loadingClass);
				}
				self.trigger('load', results);
			}]);
		},

		/**
		 * Sets the input field of the control to the specified value.
		 *
		 * @param {string} value
		 */
		setTextboxValue: function(value) {
			var $input = this.$control_input;
			var changed = $input.val() !== value;
			if (changed) {
				$input.val(value).triggerHandler('update');
				this.lastValue = value;
			}
		},

		/**
		 * Returns the value of the control. If multiple items
		 * can be selected (e.g. <select multiple>), this returns
		 * an array. If only one item can be selected, this
		 * returns a string.
		 *
		 * @returns {mixed}
		 */
		getValue: function() {
			if (this.tagType === TAG_SELECT && this.$input.attr('multiple')) {
				return this.items;
			} else {
				return this.items.join(this.settings.delimiter);
			}
		},

		/**
		 * Resets the selected items to the given value.
		 *
		 * @param {mixed} value
		 */
		setValue: function(value, silent) {
			var events = silent ? [] : ['change'];

			debounce_events(this, events, function() {
				this.clear(silent);
				this.addItems(value, silent);
			});
		},

		/**
		 * Sets the selected item.
		 *
		 * @param {object} $item
		 * @param {object} e (optional)
		 */
		setActiveItem: function($item, e) {
			var self = this;
			var eventName;
			var i, idx, begin, end, item, swap;
			var $last;

			if (self.settings.mode === 'single') return;
			$item = $($item);

			// clear the active selection
			if (!$item.length) {
				$(self.$activeItems).removeClass('active');
				self.$activeItems = [];
				if (self.isFocused) {
					self.showInput();
				}
				return;
			}

			// modify selection
			eventName = e && e.type.toLowerCase();

			if (eventName === 'mousedown' && self.isShiftDown && self.$activeItems.length) {
				$last = self.$control.children('.active:last');
				begin = Array.prototype.indexOf.apply(self.$control[0].childNodes, [$last[0]]);
				end   = Array.prototype.indexOf.apply(self.$control[0].childNodes, [$item[0]]);
				if (begin > end) {
					swap  = begin;
					begin = end;
					end   = swap;
				}
				for (i = begin; i <= end; i++) {
					item = self.$control[0].childNodes[i];
					if (self.$activeItems.indexOf(item) === -1) {
						$(item).addClass('active');
						self.$activeItems.push(item);
					}
				}
				e.preventDefault();
			} else if ((eventName === 'mousedown' && self.isCtrlDown) || (eventName === 'keydown' && this.isShiftDown)) {
				if ($item.hasClass('active')) {
					idx = self.$activeItems.indexOf($item[0]);
					self.$activeItems.splice(idx, 1);
					$item.removeClass('active');
				} else {
					self.$activeItems.push($item.addClass('active')[0]);
				}
			} else {
				$(self.$activeItems).removeClass('active');
				self.$activeItems = [$item.addClass('active')[0]];
			}

			// ensure control has focus
			self.hideInput();
			if (!this.isFocused) {
				self.focus();
			}
		},

		/**
		 * Sets the selected item in the dropdown menu
		 * of available options.
		 *
		 * @param {object} $object
		 * @param {boolean} scroll
		 * @param {boolean} animate
		 */
		setActiveOption: function($option, scroll, animate) {
			var height_menu, height_item, y;
			var scroll_top, scroll_bottom;
			var self = this;

			if (self.$activeOption) self.$activeOption.removeClass('active');
			self.$activeOption = null;

			$option = $($option);
			if (!$option.length) return;

			self.$activeOption = $option.addClass('active');

			if (scroll || !isset(scroll)) {

				height_menu   = self.$dropdown_content.height();
				height_item   = self.$activeOption.outerHeight(true);
				scroll        = self.$dropdown_content.scrollTop() || 0;
				y             = self.$activeOption.offset().top - self.$dropdown_content.offset().top + scroll;
				scroll_top    = y;
				scroll_bottom = y - height_menu + height_item;

				if (y + height_item > height_menu + scroll) {
					self.$dropdown_content.stop().animate({scrollTop: scroll_bottom}, animate ? self.settings.scrollDuration : 0);
				} else if (y < scroll) {
					self.$dropdown_content.stop().animate({scrollTop: scroll_top}, animate ? self.settings.scrollDuration : 0);
				}

			}
		},

		/**
		 * Selects all items (CTRL + A).
		 */
		selectAll: function() {
			var self = this;
			if (self.settings.mode === 'single') return;

			self.$activeItems = Array.prototype.slice.apply(self.$control.children(':not(input)').addClass('active'));
			if (self.$activeItems.length) {
				self.hideInput();
				self.close();
			}
			self.focus();
		},

		/**
		 * Hides the input element out of view, while
		 * retaining its focus.
		 */
		hideInput: function() {
			var self = this;

			self.setTextboxValue('');
			self.$control_input.css({opacity: 0, position: 'absolute', left: self.rtl ? 10000 : -10000});
			self.isInputHidden = true;
		},

		/**
		 * Restores input visibility.
		 */
		showInput: function() {
			this.$control_input.css({opacity: 1, position: 'relative', left: 0});
			this.isInputHidden = false;
		},

		/**
		 * Gives the control focus.
		 */
		focus: function() {
			var self = this;
			if (self.isDisabled) return;

			self.ignoreFocus = true;
			self.$control_input[0].focus();
			window.setTimeout(function() {
				self.ignoreFocus = false;
				self.onFocus();
			}, 0);
		},

		/**
		 * Forces the control out of focus.
		 *
		 * @param {Element} dest
		 */
		blur: function(dest) {
			this.$control_input[0].blur();
			this.onBlur(null, dest);
		},

		/**
		 * Returns a function that scores an object
		 * to show how good of a match it is to the
		 * provided query.
		 *
		 * @param {string} query
		 * @param {object} options
		 * @return {function}
		 */
		getScoreFunction: function(query) {
			return this.sifter.getScoreFunction(query, this.getSearchOptions());
		},

		/**
		 * Returns search options for sifter (the system
		 * for scoring and sorting results).
		 *
		 * @see https://github.com/brianreavis/sifter.js
		 * @return {object}
		 */
		getSearchOptions: function() {
			var settings = this.settings;
			var sort = settings.sortField;
			if (typeof sort === 'string') {
				sort = [{field: sort}];
			}

			return {
				fields      : settings.searchField,
				conjunction : settings.searchConjunction,
				sort        : sort
			};
		},

		/**
		 * Searches through available options and returns
		 * a sorted array of matches.
		 *
		 * Returns an object containing:
		 *
		 *   - query {string}
		 *   - tokens {array}
		 *   - total {int}
		 *   - items {array}
		 *
		 * @param {string} query
		 * @returns {object}
		 */
		search: function(query) {
			var i, value, score, result, calculateScore;
			var self     = this;
			var settings = self.settings;
			var options  = this.getSearchOptions();

			// validate user-provided result scoring function
			if (settings.score) {
				calculateScore = self.settings.score.apply(this, [query]);
				if (typeof calculateScore !== 'function') {
					throw new Error('Selectize "score" setting must be a function that returns a function');
				}
			}

			// perform search
			if (query !== self.lastQuery) {
				self.lastQuery = query;
				result = self.sifter.search(query, $.extend(options, {score: calculateScore}));
				self.currentResults = result;
			} else {
				result = $.extend(true, {}, self.currentResults);
			}

			// filter out selected items
			if (settings.hideSelected) {
				for (i = result.items.length - 1; i >= 0; i--) {
					if (self.items.indexOf(hash_key(result.items[i].id)) !== -1) {
						result.items.splice(i, 1);
					}
				}
			}

			return result;
		},

		/**
		 * Refreshes the list of available options shown
		 * in the autocomplete dropdown menu.
		 *
		 * @param {boolean} triggerDropdown
		 */
		refreshOptions: function(triggerDropdown) {
			var i, j, k, n, groups, groups_order, option, option_html, optgroup, optgroups, html, html_children, has_create_option;
			var $active, $active_before, $create;

			if (typeof triggerDropdown === 'undefined') {
				triggerDropdown = true;
			}

			var self              = this;
			var query             = $.trim(self.$control_input.val());
			var results           = self.search(query);
			var $dropdown_content = self.$dropdown_content;
			var active_before     = self.$activeOption && hash_key(self.$activeOption.attr('data-value'));

			// build markup
			n = results.items.length;
			if (typeof self.settings.maxOptions === 'number') {
				n = Math.min(n, self.settings.maxOptions);
			}

			// render and group available options individually
			groups = {};
			groups_order = [];

			for (i = 0; i < n; i++) {
				option      = self.options[results.items[i].id];
				option_html = self.render('option', option);
				optgroup    = option[self.settings.optgroupField] || '';
				optgroups   = $.isArray(optgroup) ? optgroup : [optgroup];

				for (j = 0, k = optgroups && optgroups.length; j < k; j++) {
					optgroup = optgroups[j];
					if (!self.optgroups.hasOwnProperty(optgroup)) {
						optgroup = '';
					}
					if (!groups.hasOwnProperty(optgroup)) {
						groups[optgroup] = [];
						groups_order.push(optgroup);
					}
					groups[optgroup].push(option_html);
				}
			}

			// sort optgroups
			if (this.settings.lockOptgroupOrder) {
				groups_order.sort(function(a, b) {
					var a_order = self.optgroups[a].$order || 0;
					var b_order = self.optgroups[b].$order || 0;
					return a_order - b_order;
				});
			}

			// render optgroup headers & join groups
			html = [];
			for (i = 0, n = groups_order.length; i < n; i++) {
				optgroup = groups_order[i];
				if (self.optgroups.hasOwnProperty(optgroup) && groups[optgroup].length) {
					// render the optgroup header and options within it,
					// then pass it to the wrapper template
					html_children = self.render('optgroup_header', self.optgroups[optgroup]) || '';
					html_children += groups[optgroup].join('');
					html.push(self.render('optgroup', $.extend({}, self.optgroups[optgroup], {
						html: html_children
					})));
				} else {
					html.push(groups[optgroup].join(''));
				}
			}

			$dropdown_content.html(html.join(''));

			// highlight matching terms inline
			if (self.settings.highlight && results.query.length && results.tokens.length) {
				for (i = 0, n = results.tokens.length; i < n; i++) {
					highlight($dropdown_content, results.tokens[i].regex);
				}
			}

			// add "selected" class to selected options
			if (!self.settings.hideSelected) {
				for (i = 0, n = self.items.length; i < n; i++) {
					self.getOption(self.items[i]).addClass('selected');
				}
			}

			// add create option
			has_create_option = self.canCreate(query);
			if (has_create_option) {
				$dropdown_content.prepend(self.render('option_create', {input: query}));
				$create = $($dropdown_content[0].childNodes[0]);
			}

			// activate
			self.hasOptions = results.items.length > 0 || has_create_option;
			if (self.hasOptions) {
				if (results.items.length > 0) {
					$active_before = active_before && self.getOption(active_before);
					if ($active_before && $active_before.length) {
						$active = $active_before;
					} else if (self.settings.mode === 'single' && self.items.length) {
						$active = self.getOption(self.items[0]);
					}
					if (!$active || !$active.length) {
						if ($create && !self.settings.addPrecedence) {
							$active = self.getAdjacentOption($create, 1);
						} else {
							$active = $dropdown_content.find('[data-selectable]:first');
						}
					}
				} else {
					$active = $create;
				}
				self.setActiveOption($active);
				if (triggerDropdown && !self.isOpen) { self.open(); }
			} else {
				self.setActiveOption(null);
				if (triggerDropdown && self.isOpen) { self.close(); }
			}
		},

		/**
		 * Adds an available option. If it already exists,
		 * nothing will happen. Note: this does not refresh
		 * the options list dropdown (use `refreshOptions`
		 * for that).
		 *
		 * Usage:
		 *
		 *   this.addOption(data)
		 *
		 * @param {object|array} data
		 */
		addOption: function(data) {
			var i, n, value, self = this;

			if ($.isArray(data)) {
				for (i = 0, n = data.length; i < n; i++) {
					self.addOption(data[i]);
				}
				return;
			}

			if (value = self.registerOption(data)) {
				self.userOptions[value] = true;
				self.lastQuery = null;
				self.trigger('option_add', value, data);
			}
		},

		/**
		 * Registers an option to the pool of options.
		 *
		 * @param {object} data
		 * @return {boolean|string}
		 */
		registerOption: function(data) {
			var key = hash_key(data[this.settings.valueField]);
			if (!key || this.options.hasOwnProperty(key)) return false;
			data.$order = data.$order || ++this.order;
			this.options[key] = data;
			return key;
		},

		/**
		 * Registers an option group to the pool of option groups.
		 *
		 * @param {object} data
		 * @return {boolean|string}
		 */
		registerOptionGroup: function(data) {
			var key = hash_key(data[this.settings.optgroupValueField]);
			if (!key) return false;

			data.$order = data.$order || ++this.order;
			this.optgroups[key] = data;
			return key;
		},

		/**
		 * Registers a new optgroup for options
		 * to be bucketed into.
		 *
		 * @param {string} id
		 * @param {object} data
		 */
		addOptionGroup: function(id, data) {
			data[this.settings.optgroupValueField] = id;
			if (id = this.registerOptionGroup(data)) {
				this.trigger('optgroup_add', id, data);
			}
		},

		/**
		 * Removes an existing option group.
		 *
		 * @param {string} id
		 */
		removeOptionGroup: function(id) {
			if (this.optgroups.hasOwnProperty(id)) {
				delete this.optgroups[id];
				this.renderCache = {};
				this.trigger('optgroup_remove', id);
			}
		},

		/**
		 * Clears all existing option groups.
		 */
		clearOptionGroups: function() {
			this.optgroups = {};
			this.renderCache = {};
			this.trigger('optgroup_clear');
		},

		/**
		 * Updates an option available for selection. If
		 * it is visible in the selected items or options
		 * dropdown, it will be re-rendered automatically.
		 *
		 * @param {string} value
		 * @param {object} data
		 */
		updateOption: function(value, data) {
			var self = this;
			var $item, $item_new;
			var value_new, index_item, cache_items, cache_options, order_old;

			value     = hash_key(value);
			value_new = hash_key(data[self.settings.valueField]);

			// sanity checks
			if (value === null) return;
			if (!self.options.hasOwnProperty(value)) return;
			if (typeof value_new !== 'string') throw new Error('Value must be set in option data');

			order_old = self.options[value].$order;

			// update references
			if (value_new !== value) {
				delete self.options[value];
				index_item = self.items.indexOf(value);
				if (index_item !== -1) {
					self.items.splice(index_item, 1, value_new);
				}
			}
			data.$order = data.$order || order_old;
			self.options[value_new] = data;

			// invalidate render cache
			cache_items = self.renderCache['item'];
			cache_options = self.renderCache['option'];

			if (cache_items) {
				delete cache_items[value];
				delete cache_items[value_new];
			}
			if (cache_options) {
				delete cache_options[value];
				delete cache_options[value_new];
			}

			// update the item if it's selected
			if (self.items.indexOf(value_new) !== -1) {
				$item = self.getItem(value);
				$item_new = $(self.render('item', data));
				if ($item.hasClass('active')) $item_new.addClass('active');
				$item.replaceWith($item_new);
			}

			// invalidate last query because we might have updated the sortField
			self.lastQuery = null;

			// update dropdown contents
			if (self.isOpen) {
				self.refreshOptions(false);
			}
		},

		/**
		 * Removes a single option.
		 *
		 * @param {string} value
		 * @param {boolean} silent
		 */
		removeOption: function(value, silent) {
			var self = this;
			value = hash_key(value);

			var cache_items = self.renderCache['item'];
			var cache_options = self.renderCache['option'];
			if (cache_items) delete cache_items[value];
			if (cache_options) delete cache_options[value];

			delete self.userOptions[value];
			delete self.options[value];
			self.lastQuery = null;
			self.trigger('option_remove', value);
			self.removeItem(value, silent);
		},

		/**
		 * Clears all options.
		 */
		clearOptions: function() {
			var self = this;

			self.loadedSearches = {};
			self.userOptions = {};
			self.renderCache = {};
			self.options = self.sifter.items = {};
			self.lastQuery = null;
			self.trigger('option_clear');
			self.clear();
		},

		/**
		 * Returns the jQuery element of the option
		 * matching the given value.
		 *
		 * @param {string} value
		 * @returns {object}
		 */
		getOption: function(value) {
			return this.getElementWithValue(value, this.$dropdown_content.find('[data-selectable]'));
		},

		/**
		 * Returns the jQuery element of the next or
		 * previous selectable option.
		 *
		 * @param {object} $option
		 * @param {int} direction  can be 1 for next or -1 for previous
		 * @return {object}
		 */
		getAdjacentOption: function($option, direction) {
			var $options = this.$dropdown.find('[data-selectable]');
			var index    = $options.index($option) + direction;

			return index >= 0 && index < $options.length ? $options.eq(index) : $();
		},

		/**
		 * Finds the first element with a "data-value" attribute
		 * that matches the given value.
		 *
		 * @param {mixed} value
		 * @param {object} $els
		 * @return {object}
		 */
		getElementWithValue: function(value, $els) {
			value = hash_key(value);

			if (typeof value !== 'undefined' && value !== null) {
				for (var i = 0, n = $els.length; i < n; i++) {
					if ($els[i].getAttribute('data-value') === value) {
						return $($els[i]);
					}
				}
			}

			return $();
		},

		/**
		 * Returns the jQuery element of the item
		 * matching the given value.
		 *
		 * @param {string} value
		 * @returns {object}
		 */
		getItem: function(value) {
			return this.getElementWithValue(value, this.$control.children());
		},

		/**
		 * "Selects" multiple items at once. Adds them to the list
		 * at the current caret position.
		 *
		 * @param {string} value
		 * @param {boolean} silent
		 */
		addItems: function(values, silent) {
			var items = $.isArray(values) ? values : [values];
			for (var i = 0, n = items.length; i < n; i++) {
				this.isPending = (i < n - 1);
				this.addItem(items[i], silent);
			}
		},

		/**
		 * "Selects" an item. Adds it to the list
		 * at the current caret position.
		 *
		 * @param {string} value
		 * @param {boolean} silent
		 */
		addItem: function(value, silent) {
			var events = silent ? [] : ['change'];

			debounce_events(this, events, function() {
				var $item, $option, $options;
				var self = this;
				var inputMode = self.settings.mode;
				var i, active, value_next, wasFull;
				value = hash_key(value);

				if (self.items.indexOf(value) !== -1) {
					if (inputMode === 'single') self.close();
					return;
				}

				if (!self.options.hasOwnProperty(value)) return;
				if (inputMode === 'single') self.clear(silent);
				if (inputMode === 'multi' && self.isFull()) return;

				$item = $(self.render('item', self.options[value]));
				wasFull = self.isFull();
				self.items.splice(self.caretPos, 0, value);
				self.insertAtCaret($item);
				if (!self.isPending || (!wasFull && self.isFull())) {
					self.refreshState();
				}

				if (self.isSetup) {
					$options = self.$dropdown_content.find('[data-selectable]');

					// update menu / remove the option (if this is not one item being added as part of series)
					if (!self.isPending) {
						$option = self.getOption(value);
						value_next = self.getAdjacentOption($option, 1).attr('data-value');
						self.refreshOptions(self.isFocused && inputMode !== 'single');
						if (value_next) {
							self.setActiveOption(self.getOption(value_next));
						}
					}

					// hide the menu if the maximum number of items have been selected or no options are left
					if (!$options.length || self.isFull()) {
						self.close();
					} else {
						self.positionDropdown();
					}

					self.updatePlaceholder();
					self.trigger('item_add', value, $item);
					self.updateOriginalInput({silent: silent});
				}
			});
		},

		/**
		 * Removes the selected item matching
		 * the provided value.
		 *
		 * @param {string} value
		 */
		removeItem: function(value, silent) {
			var self = this;
			var $item, i, idx;

			$item = (typeof value === 'object') ? value : self.getItem(value);
			value = hash_key($item.attr('data-value'));
			i = self.items.indexOf(value);

			if (i !== -1) {
				$item.remove();
				if ($item.hasClass('active')) {
					idx = self.$activeItems.indexOf($item[0]);
					self.$activeItems.splice(idx, 1);
				}

				self.items.splice(i, 1);
				self.lastQuery = null;
				if (!self.settings.persist && self.userOptions.hasOwnProperty(value)) {
					self.removeOption(value, silent);
				}

				if (i < self.caretPos) {
					self.setCaret(self.caretPos - 1);
				}

				self.refreshState();
				self.updatePlaceholder();
				self.updateOriginalInput({silent: silent});
				self.positionDropdown();
				self.trigger('item_remove', value, $item);
			}
		},

		/**
		 * Invokes the `create` method provided in the
		 * selectize options that should provide the data
		 * for the new item, given the user input.
		 *
		 * Once this completes, it will be added
		 * to the item list.
		 *
		 * @param {string} value
		 * @param {boolean} [triggerDropdown]
		 * @param {function} [callback]
		 * @return {boolean}
		 */
		createItem: function(input, triggerDropdown) {
			var self  = this;
			var caret = self.caretPos;
			input = input || $.trim(self.$control_input.val() || '');

			var callback = arguments[arguments.length - 1];
			if (typeof callback !== 'function') callback = function() {};

			if (typeof triggerDropdown !== 'boolean') {
				triggerDropdown = true;
			}

			if (!self.canCreate(input)) {
				callback();
				return false;
			}

			self.lock();

			var setup = (typeof self.settings.create === 'function') ? this.settings.create : function(input) {
				var data = {};
				data[self.settings.labelField] = input;
				data[self.settings.valueField] = input;
				return data;
			};

			var create = once(function(data) {
				self.unlock();

				if (!data || typeof data !== 'object') return callback();
				var value = hash_key(data[self.settings.valueField]);
				if (typeof value !== 'string') return callback();

				self.setTextboxValue('');
				self.addOption(data);
				self.setCaret(caret);
				self.addItem(value);
				self.refreshOptions(triggerDropdown && self.settings.mode !== 'single');
				callback(data);
			});

			var output = setup.apply(this, [input, create]);
			if (typeof output !== 'undefined') {
				create(output);
			}

			return true;
		},

		/**
		 * Re-renders the selected item lists.
		 */
		refreshItems: function() {
			this.lastQuery = null;

			if (this.isSetup) {
				this.addItem(this.items);
			}

			this.refreshState();
			this.updateOriginalInput();
		},

		/**
		 * Updates all state-dependent attributes
		 * and CSS classes.
		 */
		refreshState: function() {
			var invalid, self = this;
			if (self.isRequired) {
				if (self.items.length) self.isInvalid = false;
				self.$control_input.prop('required', invalid);
			}
			self.refreshClasses();
		},

		/**
		 * Updates all state-dependent CSS classes.
		 */
		refreshClasses: function() {
			var self     = this;
			var isFull   = self.isFull();
			var isLocked = self.isLocked;

			self.$wrapper
				.toggleClass('rtl', self.rtl);

			self.$control
				.toggleClass('focus', self.isFocused)
				.toggleClass('disabled', self.isDisabled)
				.toggleClass('required', self.isRequired)
				.toggleClass('invalid', self.isInvalid)
				.toggleClass('locked', isLocked)
				.toggleClass('full', isFull).toggleClass('not-full', !isFull)
				.toggleClass('input-active', self.isFocused && !self.isInputHidden)
				.toggleClass('dropdown-active', self.isOpen)
				.toggleClass('has-options', !$.isEmptyObject(self.options))
				.toggleClass('has-items', self.items.length > 0);

			self.$control_input.data('grow', !isFull && !isLocked);
		},

		/**
		 * Determines whether or not more items can be added
		 * to the control without exceeding the user-defined maximum.
		 *
		 * @returns {boolean}
		 */
		isFull: function() {
			return this.settings.maxItems !== null && this.items.length >= this.settings.maxItems;
		},

		/**
		 * Refreshes the original <select> or <input>
		 * element to reflect the current state.
		 */
		updateOriginalInput: function(opts) {
			var i, n, options, label, self = this;
			opts = opts || {};

			if (self.tagType === TAG_SELECT) {
				options = [];
				for (i = 0, n = self.items.length; i < n; i++) {
					label = self.options[self.items[i]][self.settings.labelField] || '';
					options.push('<option value="' + escape_html(self.items[i]) + '" selected="selected">' + escape_html(label) + '</option>');
				}
				if (!options.length && !this.$input.attr('multiple')) {
					options.push('<option value="" selected="selected"></option>');
				}
				self.$input.html(options.join(''));
			} else {
				self.$input.val(self.getValue());
				self.$input.attr('value',self.$input.val());
			}

			if (self.isSetup) {
				if (!opts.silent) {
					self.trigger('change', self.$input.val());
				}
			}
		},

		/**
		 * Shows/hide the input placeholder depending
		 * on if there items in the list already.
		 */
		updatePlaceholder: function() {
			if (!this.settings.placeholder) return;
			var $input = this.$control_input;

			if (this.items.length) {
				$input.removeAttr('placeholder');
			} else {
				$input.attr('placeholder', this.settings.placeholder);
			}
			$input.triggerHandler('update', {force: true});
		},

		/**
		 * Shows the autocomplete dropdown containing
		 * the available options.
		 */
		open: function() {
			var self = this;

			if (self.isLocked || self.isOpen || (self.settings.mode === 'multi' && self.isFull())) return;
			self.focus();
			self.isOpen = true;
			self.refreshState();
			self.$dropdown.css({visibility: 'hidden', display: 'block'});
			self.positionDropdown();
			self.$dropdown.css({visibility: 'visible'});
			self.trigger('dropdown_open', self.$dropdown);

			//! Amethyst block
			self.$dropdown.addClass('selectize-dropdown-active');
			if(jQuery().scrollbar && self.$dropdown.find('.scroll-content').length === 0) {
				// Initialize scrollbar
				self.$dropdown.find('.selectize-dropdown-content').scrollbar({ignoreMobile: false});
			}
			//! /Amethyst block
		},

		/**
		 * Closes the autocomplete dropdown menu.
		 */
		close: function() {
			var self = this;
			var trigger = self.isOpen;

			if (self.settings.mode === 'single' && self.items.length) {
				self.hideInput();
			}

			self.isOpen = false;
			self.$dropdown.hide();
			self.setActiveOption(null);
			self.refreshState();

			if (trigger) self.trigger('dropdown_close', self.$dropdown);

			//! Amethyst block
			self.$dropdown.removeClass('selectize-dropdown-active');
			//! /Amethyst block
		},

		/**
		 * Calculates and applies the appropriate
		 * position of the dropdown.
		 */
		positionDropdown: function() {
			var $control = this.$control;
			var offset = this.settings.dropdownParent === 'body' ? $control.offset() : $control.position();
			offset.top += $control.outerHeight(true);

			this.$dropdown.css({
				width : $control.outerWidth(),
				top   : offset.top,
				left  : offset.left
			});
		},

		/**
		 * Resets / clears all selected items
		 * from the control.
		 *
		 * @param {boolean} silent
		 */
		clear: function(silent) {
			var self = this;

			if (!self.items.length) return;
			self.$control.children(':not(input)').remove();
			self.items = [];
			self.lastQuery = null;
			self.setCaret(0);
			self.setActiveItem(null);
			self.updatePlaceholder();
			self.updateOriginalInput({silent: silent});
			self.refreshState();
			self.showInput();
			self.trigger('clear');
		},

		/**
		 * A helper method for inserting an element
		 * at the current caret position.
		 *
		 * @param {object} $el
		 */
		insertAtCaret: function($el) {
			var caret = Math.min(this.caretPos, this.items.length);
			if (caret === 0) {
				this.$control.prepend($el);
			} else {
				$(this.$control[0].childNodes[caret]).before($el);
			}
			this.setCaret(caret + 1);
		},

		/**
		 * Removes the current selected item(s).
		 *
		 * @param {object} e (optional)
		 * @returns {boolean}
		 */
		deleteSelection: function(e) {
			var i, n, direction, selection, values, caret, option_select, $option_select, $tail;
			var self = this;

			direction = (e && e.keyCode === KEY_BACKSPACE) ? -1 : 1;
			selection = getSelection(self.$control_input[0]);

			if (self.$activeOption && !self.settings.hideSelected) {
				option_select = self.getAdjacentOption(self.$activeOption, -1).attr('data-value');
			}

			// determine items that will be removed
			values = [];

			if (self.$activeItems.length) {
				$tail = self.$control.children('.active:' + (direction > 0 ? 'last' : 'first'));
				caret = self.$control.children(':not(input)').index($tail);
				if (direction > 0) { caret++; }

				for (i = 0, n = self.$activeItems.length; i < n; i++) {
					values.push($(self.$activeItems[i]).attr('data-value'));
				}
				if (e) {
					e.preventDefault();
					e.stopPropagation();
				}
			} else if ((self.isFocused || self.settings.mode === 'single') && self.items.length) {
				if (direction < 0 && selection.start === 0 && selection.length === 0) {
					values.push(self.items[self.caretPos - 1]);
				} else if (direction > 0 && selection.start === self.$control_input.val().length) {
					values.push(self.items[self.caretPos]);
				}
			}

			// allow the callback to abort
			if (!values.length || (typeof self.settings.onDelete === 'function' && self.settings.onDelete.apply(self, [values]) === false)) {
				return false;
			}

			// perform removal
			if (typeof caret !== 'undefined') {
				self.setCaret(caret);
			}
			while (values.length) {
				self.removeItem(values.pop());
			}

			self.showInput();
			self.positionDropdown();
			self.refreshOptions(true);

			// select previous option
			if (option_select) {
				$option_select = self.getOption(option_select);
				if ($option_select.length) {
					self.setActiveOption($option_select);
				}
			}

			return true;
		},

		/**
		 * Selects the previous / next item (depending
		 * on the `direction` argument).
		 *
		 * > 0 - right
		 * < 0 - left
		 *
		 * @param {int} direction
		 * @param {object} e (optional)
		 */
		advanceSelection: function(direction, e) {
			var tail, selection, idx, valueLength, cursorAtEdge, $tail;
			var self = this;

			if (direction === 0) return;
			if (self.rtl) direction *= -1;

			tail = direction > 0 ? 'last' : 'first';
			selection = getSelection(self.$control_input[0]);

			if (self.isFocused && !self.isInputHidden) {
				valueLength = self.$control_input.val().length;
				cursorAtEdge = direction < 0
					? selection.start === 0 && selection.length === 0
					: selection.start === valueLength;

				if (cursorAtEdge && !valueLength) {
					self.advanceCaret(direction, e);
				}
			} else {
				$tail = self.$control.children('.active:' + tail);
				if ($tail.length) {
					idx = self.$control.children(':not(input)').index($tail);
					self.setActiveItem(null);
					self.setCaret(direction > 0 ? idx + 1 : idx);
				}
			}
		},

		/**
		 * Moves the caret left / right.
		 *
		 * @param {int} direction
		 * @param {object} e (optional)
		 */
		advanceCaret: function(direction, e) {
			var self = this, fn, $adj;

			if (direction === 0) return;

			fn = direction > 0 ? 'next' : 'prev';
			if (self.isShiftDown) {
				$adj = self.$control_input[fn]();
				if ($adj.length) {
					self.hideInput();
					self.setActiveItem($adj);
					e && e.preventDefault();
				}
			} else {
				self.setCaret(self.caretPos + direction);
			}
		},

		/**
		 * Moves the caret to the specified index.
		 *
		 * @param {int} i
		 */
		setCaret: function(i) {
			var self = this;

			if (self.settings.mode === 'single') {
				i = self.items.length;
			} else {
				i = Math.max(0, Math.min(self.items.length, i));
			}

			if(!self.isPending) {
				// the input must be moved by leaving it in place and moving the
				// siblings, due to the fact that focus cannot be restored once lost
				// on mobile webkit devices
				var j, n, fn, $children, $child;
				$children = self.$control.children(':not(input)');
				for (j = 0, n = $children.length; j < n; j++) {
					$child = $($children[j]).detach();
					if (j <  i) {
						self.$control_input.before($child);
					} else {
						self.$control.append($child);
					}
				}
			}

			self.caretPos = i;
		},

		/**
		 * Disables user input on the control. Used while
		 * items are being asynchronously created.
		 */
		lock: function() {
			this.close();
			this.isLocked = true;
			this.refreshState();
		},

		/**
		 * Re-enables user input on the control.
		 */
		unlock: function() {
			this.isLocked = false;
			this.refreshState();
		},

		/**
		 * Disables user input on the control completely.
		 * While disabled, it cannot receive focus.
		 */
		disable: function() {
			var self = this;
			self.$input.prop('disabled', true);
			self.$control_input.prop('disabled', true).prop('tabindex', -1);
			self.isDisabled = true;
			self.lock();
		},

		/**
		 * Enables the control so that it can respond
		 * to focus and user input.
		 */
		enable: function() {
			var self = this;
			self.$input.prop('disabled', false);
			self.$control_input.prop('disabled', false).prop('tabindex', self.tabIndex);
			self.isDisabled = false;
			self.unlock();
		},

		/**
		 * Completely destroys the control and
		 * unbinds all event listeners so that it can
		 * be garbage collected.
		 */
		destroy: function() {
			var self = this;
			var eventNS = self.eventNS;
			var revertSettings = self.revertSettings;

			self.trigger('destroy');
			self.off();
			self.$wrapper.remove();
			self.$dropdown.remove();

			self.$input
				.html('')
				.append(revertSettings.$children)
				.removeAttr('tabindex')
				.removeClass('selectized')
				.attr({tabindex: revertSettings.tabindex})
				.show();

			self.$control_input.removeData('grow');
			self.$input.removeData('selectize');

			$(window).off(eventNS);
			$(document).off(eventNS);
			$(document.body).off(eventNS);

			delete self.$input[0].selectize;
		},

		/**
		 * A helper method for rendering "item" and
		 * "option" templates, given the data.
		 *
		 * @param {string} templateName
		 * @param {object} data
		 * @returns {string}
		 */
		render: function(templateName, data) {
			var value, id, label;
			var html = '';
			var cache = false;
			var self = this;
			var regex_tag = /^[\t \r\n]*<([a-z][a-z0-9\-_]*(?:\:[a-z][a-z0-9\-_]*)?)/i;

			if (templateName === 'option' || templateName === 'item') {
				value = hash_key(data[self.settings.valueField]);
				cache = !!value;
			}

			// pull markup from cache if it exists
			if (cache) {
				if (!isset(self.renderCache[templateName])) {
					self.renderCache[templateName] = {};
				}
				if (self.renderCache[templateName].hasOwnProperty(value)) {
					return self.renderCache[templateName][value];
				}
			}

			// render markup
			html = self.settings.render[templateName].apply(this, [data, escape_html]);

			// add mandatory attributes
			if (templateName === 'option' || templateName === 'option_create') {
				html = html.replace(regex_tag, '<$1 data-selectable');
			}
			if (templateName === 'optgroup') {
				id = data[self.settings.optgroupValueField] || '';
				html = html.replace(regex_tag, '<$1 data-group="' + escape_replace(escape_html(id)) + '"');
			}
			if (templateName === 'option' || templateName === 'item') {
				html = html.replace(regex_tag, '<$1 data-value="' + escape_replace(escape_html(value || '')) + '"');
			}

			// update cache
			if (cache) {
				self.renderCache[templateName][value] = html;
			}

			return html;
		},

		/**
		 * Clears the render cache for a template. If
		 * no template is given, clears all render
		 * caches.
		 *
		 * @param {string} templateName
		 */
		clearCache: function(templateName) {
			var self = this;
			if (typeof templateName === 'undefined') {
				self.renderCache = {};
			} else {
				delete self.renderCache[templateName];
			}
		},

		/**
		 * Determines whether or not to display the
		 * create item prompt, given a user input.
		 *
		 * @param {string} input
		 * @return {boolean}
		 */
		canCreate: function(input) {
			var self = this;
			if (!self.settings.create) return false;
			var filter = self.settings.createFilter;
			return input.length
				&& (typeof filter !== 'function' || filter.apply(self, [input]))
				&& (typeof filter !== 'string' || new RegExp(filter).test(input))
				&& (!(filter instanceof RegExp) || filter.test(input));
		}

	});
	
	
	Selectize.count = 0;
	Selectize.defaults = {
		options: [],
		optgroups: [],

		plugins: [],
		delimiter: ',',
		splitOn: null, // regexp or string for splitting up values from a paste command
		persist: true,
		diacritics: true,
		create: false,
		createOnBlur: false,
		createFilter: null,
		highlight: true,
		openOnFocus: true,
		maxOptions: 1000,
		maxItems: null,
		hideSelected: null,
		addPrecedence: false,
		selectOnTab: false,
		preload: false,
		allowEmptyOption: false,
		closeAfterSelect: false,

		scrollDuration: 60,
		loadThrottle: 300,
		loadingClass: 'loading',

		dataAttr: 'data-data',
		optgroupField: 'optgroup',
		valueField: 'value',
		labelField: 'text',
		optgroupLabelField: 'label',
		optgroupValueField: 'value',
		lockOptgroupOrder: false,

		sortField: '$order',
		searchField: ['text'],
		searchConjunction: 'and',

		mode: null,
		wrapperClass: 'selectize-control',
		inputClass: 'selectize-input',
		dropdownClass: 'selectize-dropdown',
		dropdownContentClass: 'selectize-dropdown-content',

		dropdownParent: null,

		copyClassesToDropdown: true,

		/*
		 load                 : null, // function(query, callback) { ... }
		 score                : null, // function(search) { ... }
		 onInitialize         : null, // function() { ... }
		 onChange             : null, // function(value) { ... }
		 onItemAdd            : null, // function(value, $item) { ... }
		 onItemRemove         : null, // function(value) { ... }
		 onClear              : null, // function() { ... }
		 onOptionAdd          : null, // function(value, data) { ... }
		 onOptionRemove       : null, // function(value) { ... }
		 onOptionClear        : null, // function() { ... }
		 onOptionGroupAdd     : null, // function(id, data) { ... }
		 onOptionGroupRemove  : null, // function(id) { ... }
		 onOptionGroupClear   : null, // function() { ... }
		 onDropdownOpen       : null, // function($dropdown) { ... }
		 onDropdownClose      : null, // function($dropdown) { ... }
		 onType               : null, // function(str) { ... }
		 onDelete             : null, // function(values) { ... }
		 */

		render: {
			/*
			 item: null,
			 optgroup: null,
			 optgroup_header: null,
			 option: null,
			 option_create: null
			 */
		}
	};
	
	
	$.fn.selectize = function(settings_user) {
		var defaults             = $.fn.selectize.defaults;
		var settings             = $.extend({}, defaults, settings_user);
		var attr_data            = settings.dataAttr;
		var field_label          = settings.labelField;
		var field_value          = settings.valueField;
		var field_optgroup       = settings.optgroupField;
		var field_optgroup_label = settings.optgroupLabelField;
		var field_optgroup_value = settings.optgroupValueField;

		/**
		 * Initializes selectize from a <input type="text"> element.
		 *
		 * @param {object} $input
		 * @param {object} settings_element
		 */
		var init_textbox = function($input, settings_element) {
			var i, n, values, option;

			var data_raw = $input.attr(attr_data);

			if (!data_raw) {
				var value = $.trim($input.val() || '');
				if (!settings.allowEmptyOption && !value.length) return;
				values = value.split(settings.delimiter);
				for (i = 0, n = values.length; i < n; i++) {
					option = {};
					option[field_label] = values[i];
					option[field_value] = values[i];
					settings_element.options.push(option);
				}
				settings_element.items = values;
			} else {
				settings_element.options = JSON.parse(data_raw);
				for (i = 0, n = settings_element.options.length; i < n; i++) {
					settings_element.items.push(settings_element.options[i][field_value]);
				}
			}
		};

		/**
		 * Initializes selectize from a <select> element.
		 *
		 * @param {object} $input
		 * @param {object} settings_element
		 */
		var init_select = function($input, settings_element) {
			var i, n, tagName, $children, order = 0;
			var options = settings_element.options;
			var optionsMap = {};

			var readData = function($el) {
				var data = attr_data && $el.attr(attr_data);
				if (typeof data === 'string' && data.length) {
					return JSON.parse(data);
				}
				return null;
			};

			var addOption = function($option, group) {
				$option = $($option);

				var value = hash_key($option.attr('value'));
				if (!value && !settings.allowEmptyOption) return;

				// if the option already exists, it's probably been
				// duplicated in another optgroup. in this case, push
				// the current group to the "optgroup" property on the
				// existing option so that it's rendered in both places.
				if (optionsMap.hasOwnProperty(value)) {
					if (group) {
						var arr = optionsMap[value][field_optgroup];
						if (!arr) {
							optionsMap[value][field_optgroup] = group;
						} else if (!$.isArray(arr)) {
							optionsMap[value][field_optgroup] = [arr, group];
						} else {
							arr.push(group);
						}
					}
					return;
				}

				var option             = readData($option) || {};
				option[field_label]    = option[field_label] || $option.text();
				option[field_value]    = option[field_value] || value;
				option[field_optgroup] = option[field_optgroup] || group;

				optionsMap[value] = option;
				options.push(option);

				if ($option.is(':selected')) {
					settings_element.items.push(value);
				}
			};

			var addGroup = function($optgroup) {
				var i, n, id, optgroup, $options;

				$optgroup = $($optgroup);
				id = $optgroup.attr('label');

				if (id) {
					optgroup = readData($optgroup) || {};
					optgroup[field_optgroup_label] = id;
					optgroup[field_optgroup_value] = id;
					settings_element.optgroups.push(optgroup);
				}

				$options = $('option', $optgroup);
				for (i = 0, n = $options.length; i < n; i++) {
					addOption($options[i], id);
				}
			};

			settings_element.maxItems = $input.attr('multiple') ? null : 1;

			$children = $input.children();
			for (i = 0, n = $children.length; i < n; i++) {
				tagName = $children[i].tagName.toLowerCase();
				if (tagName === 'optgroup') {
					addGroup($children[i]);
				} else if (tagName === 'option') {
					addOption($children[i]);
				}
			}
		};

		return this.each(function() {
			if (this.selectize) return;

			var instance;
			var $input = $(this);
			var tag_name = this.tagName.toLowerCase();
			var placeholder = $input.attr('placeholder') || $input.attr('data-placeholder');
			if (!placeholder && !settings.allowEmptyOption) {
				placeholder = $input.children('option[value=""]').text();
			}

			var settings_element = {
				'placeholder' : placeholder,
				'options'     : [],
				'optgroups'   : [],
				'items'       : []
			};

			if (tag_name === 'select') {
				init_select($input, settings_element);
			} else {
				init_textbox($input, settings_element);
			}

			instance = new Selectize($input, $.extend(true, {}, defaults, settings_element, settings_user));
		});
	};
	
	$.fn.selectize.defaults = Selectize.defaults;
	$.fn.selectize.support = {
		validity: SUPPORTS_VALIDITY_API
	};
	
	
	Selectize.define('drag_drop', function(options) {
		if (!$.fn.sortable) throw new Error('The "drag_drop" plugin requires jQuery UI "sortable".');
		if (this.settings.mode !== 'multi') return;
		var self = this;

		self.lock = (function() {
			var original = self.lock;
			return function() {
				var sortable = self.$control.data('sortable');
				if (sortable) sortable.disable();
				return original.apply(self, arguments);
			};
		})();

		self.unlock = (function() {
			var original = self.unlock;
			return function() {
				var sortable = self.$control.data('sortable');
				if (sortable) sortable.enable();
				return original.apply(self, arguments);
			};
		})();

		self.setup = (function() {
			var original = self.setup;
			return function() {
				original.apply(this, arguments);

				var $control = self.$control.sortable({
					items: '[data-value]',
					forcePlaceholderSize: true,
					disabled: self.isLocked,
					start: function(e, ui) {
						ui.placeholder.css('width', ui.helper.css('width'));
						$control.css({overflow: 'visible'});
					},
					stop: function() {
						$control.css({overflow: 'hidden'});
						var active = self.$activeItems ? self.$activeItems.slice() : null;
						var values = [];
						$control.children('[data-value]').each(function() {
							values.push($(this).attr('data-value'));
						});
						self.setValue(values);
						self.setActiveItem(active);
					}
				});
			};
		})();

	});
	
	Selectize.define('dropdown_header', function(options) {
		var self = this;

		options = $.extend({
			title         : 'Untitled',
			headerClass   : 'selectize-dropdown-header',
			titleRowClass : 'selectize-dropdown-header-title',
			labelClass    : 'selectize-dropdown-header-label',
			closeClass    : 'selectize-dropdown-header-close',

			html: function(data) {
				return (
					'<div class="' + data.headerClass + '">' +
					'<div class="' + data.titleRowClass + '">' +
					'<span class="' + data.labelClass + '">' + data.title + '</span>' +
					'<a href="javascript:void(0)" class="' + data.closeClass + '">&times;</a>' +
					'</div>' +
					'</div>'
				);
			}
		}, options);

		self.setup = (function() {
			var original = self.setup;
			return function() {
				original.apply(self, arguments);
				self.$dropdown_header = $(options.html(options));
				self.$dropdown.prepend(self.$dropdown_header);
			};
		})();

	});
	
	Selectize.define('optgroup_columns', function(options) {
		var self = this;

		options = $.extend({
			equalizeWidth  : true,
			equalizeHeight : true
		}, options);

		this.getAdjacentOption = function($option, direction) {
			var $options = $option.closest('[data-group]').find('[data-selectable]');
			var index    = $options.index($option) + direction;

			return index >= 0 && index < $options.length ? $options.eq(index) : $();
		};

		this.onKeyDown = (function() {
			var original = self.onKeyDown;
			return function(e) {
				var index, $option, $options, $optgroup;

				if (this.isOpen && (e.keyCode === KEY_LEFT || e.keyCode === KEY_RIGHT)) {
					self.ignoreHover = true;
					$optgroup = this.$activeOption.closest('[data-group]');
					index = $optgroup.find('[data-selectable]').index(this.$activeOption);

					if(e.keyCode === KEY_LEFT) {
						$optgroup = $optgroup.prev('[data-group]');
					} else {
						$optgroup = $optgroup.next('[data-group]');
					}

					$options = $optgroup.find('[data-selectable]');
					$option  = $options.eq(Math.min($options.length - 1, index));
					if ($option.length) {
						this.setActiveOption($option);
					}
					return;
				}

				return original.apply(this, arguments);
			};
		})();

		var getScrollbarWidth = function() {
			var div;
			var width = getScrollbarWidth.width;
			var doc = document;

			if (typeof width === 'undefined') {
				div = doc.createElement('div');
				div.innerHTML = '<div style="width:50px;height:50px;position:absolute;left:-50px;top:-50px;overflow:auto;"><div style="width:1px;height:100px;"></div></div>';
				div = div.firstChild;
				doc.body.appendChild(div);
				width = getScrollbarWidth.width = div.offsetWidth - div.clientWidth;
				doc.body.removeChild(div);
			}
			return width;
		};

		var equalizeSizes = function() {
			var i, n, height_max, width, width_last, width_parent, $optgroups;

			$optgroups = $('[data-group]', self.$dropdown_content);
			n = $optgroups.length;
			if (!n || !self.$dropdown_content.width()) return;

			if (options.equalizeHeight) {
				height_max = 0;
				for (i = 0; i < n; i++) {
					height_max = Math.max(height_max, $optgroups.eq(i).height());
				}
				$optgroups.css({height: height_max});
			}

			if (options.equalizeWidth) {
				width_parent = self.$dropdown_content.innerWidth() - getScrollbarWidth();
				width = Math.round(width_parent / n);
				$optgroups.css({width: width});
				if (n > 1) {
					width_last = width_parent - width * (n - 1) - 1;
					$optgroups.eq(n - 1).css({width: width_last});
				}
			}
		};

		if (options.equalizeHeight || options.equalizeWidth) {
			hook.after(this, 'positionDropdown', equalizeSizes);
			hook.after(this, 'refreshOptions', equalizeSizes);
		}


	});
	
	Selectize.define('remove_button', function(options) {
		if (this.settings.mode === 'single') return;

		options = $.extend({
			label     : '&times;',
			title     : 'Remove',
			className : 'remove',
			append    : true
		}, options);

		var self = this;
		var html = '<a href="javascript:void(0)" class="' + options.className + '" tabindex="-1" title="' + escape_html(options.title) + '">' + options.label + '</a>';

		/**
		 * Appends an element as a child (with raw HTML).
		 *
		 * @param {string} html_container
		 * @param {string} html_element
		 * @return {string}
		 */
		var append = function(html_container, html_element) {
			var pos = html_container.search(/(<\/[^>]+>\s*)$/);
			return html_container.substring(0, pos) + html_element + html_container.substring(pos);
		};

		this.setup = (function() {
			var original = self.setup;
			return function() {
				// override the item rendering method to add the button to each
				if (options.append) {
					var render_item = self.settings.render.item;
					self.settings.render.item = function(data) {
						return append(render_item.apply(this, arguments), html);
					};
				}

				original.apply(this, arguments);

				// add event listener
				this.$control.on('click', '.' + options.className, function(e) {
					e.preventDefault();
					if (self.isLocked) return;

					var $item = $(e.currentTarget).parent();
					self.setActiveItem($item);
					if (self.deleteSelection()) {
						self.setCaret(self.items.length);
					}
				});

			};
		})();

	});
	
	Selectize.define('restore_on_backspace', function(options) {
		var self = this;

		options.text = options.text || function(option) {
				return option[this.settings.labelField];
			};

		this.onKeyDown = (function() {
			var original = self.onKeyDown;
			return function(e) {
				var index, option;
				if (e.keyCode === KEY_BACKSPACE && this.$control_input.val() === '' && !this.$activeItems.length) {
					index = this.caretPos - 1;
					if (index >= 0 && index < this.items.length) {
						option = this.options[this.items[index]];
						if (this.deleteSelection(e)) {
							this.setTextboxValue(options.text.apply(this, [option]));
							this.refreshOptions(true);
						}
						e.preventDefault();
						return;
					}
				}
				return original.apply(this, arguments);
			};
		})();
	});
	

	return Selectize;
}));
// require "shards/dropdown/_dropdown.js"
/**
 * jQuery CSS Customizable Scrollbar
 *
 * Copyright 2014, Yuriy Khabarov
 * Dual licensed under the MIT or GPL Version 2 licenses.
 *
 * If you found bug, please contact me via email <13real008@gmail.com>
 *
 * @author Yuriy Khabarov aka Gromo
 * @version 0.2.5
 * @url https://github.com/gromo/jquery.scrollbar/
 *
 */
;
(function($, doc, win){
    'use strict';

    // init flags & variables
    var debug = false;
    var lmb = 1, px = "px";

    var browser = {
        "data": {},
        "macosx": win.navigator.platform.toLowerCase().indexOf('mac') !== -1,
        "mobile": /Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(win.navigator.userAgent),
        "overlay": null,
        "scroll": null,
        "scrolls": [],
        "webkit": /WebKit/.test(win.navigator.userAgent),

        "log": debug ? function(data, toString){
            var output = data;
            if(toString && typeof data != "string"){
                output = [];
                $.each(data, function(i, v){
                    output.push('"' + i + '": ' + v);
                });
                output = output.join(", ");
            }
            if(win.console && win.console.log){
                win.console.log(output);
            } else {
                alert(output);
            }
        } : function(){

        }
    };

    var defaults = {
        "autoScrollSize": true,     // automatically calculate scrollsize
        "autoUpdate": true,         // update scrollbar if content/container size changed
        "debug": false,             // debug mode
        "disableBodyScroll": false, // disable body scroll if mouse over container
        "duration": 200,            // scroll animate duration in ms
        "ignoreMobile": false,       // ignore mobile devices
        "ignoreOverlay": false,      // ignore browsers with overlay scrollbars (mobile, MacOS)
        "scrollStep": 30,           // scroll step for scrollbar arrows
        "showArrows": false,        // add class to show arrows
        "stepScrolling": true,      // when scrolling to scrollbar mousedown position
        "type":"simple",            // [advanced|simple] scrollbar html type

        "scrollx": null,            // horizontal scroll element
        "scrolly": null,            // vertical scroll element

        "onDestroy": null,          // callback function on destroy,
        "onInit": null,             // callback function on first initialization
        "onScroll": null,           // callback function on content scrolling
        "onUpdate": null            // callback function on init/resize (before scrollbar size calculation)
    };


    var customScrollbar = function(container, options){

        if(!browser.scroll){
            browser.log("Init jQuery Scrollbar v0.2.5");
            browser.overlay = isScrollOverlaysContent();
            browser.scroll = getBrowserScrollSize();
            updateScrollbars();

            $(win).resize(function(){
                var forceUpdate = false;
                if(browser.scroll && (browser.scroll.height || browser.scroll.width)){
                    var scroll = getBrowserScrollSize();
                    if(scroll.height != browser.scroll.height || scroll.width != browser.scroll.width){
                        browser.scroll = scroll;
                        forceUpdate = true; // handle page zoom
                    }
                }
                updateScrollbars(forceUpdate);
            });
        }

        this.container = container;
        this.options = $.extend({}, defaults, win.jQueryScrollbarOptions || {});
        this.scrollTo = null;
        this.scrollx = {};
        this.scrolly = {};

        this.init(options);
    };

    customScrollbar.prototype = {

        destroy: function(){

            if(!this.wrapper){
                return;
            }

            // init variables
            var scrollLeft = this.container.scrollLeft();
            var scrollTop  = this.container.scrollTop();

            this.container.insertBefore(this.wrapper).css({
                "height":"",
                "margin":""
            })
            .removeClass("scroll-content")
            .removeClass("scroll-scrollx_visible")
            .removeClass("scroll-scrolly_visible")
            .off(".scrollbar")
            .scrollLeft(scrollLeft)
            .scrollTop(scrollTop);

            this.scrollx.scrollbar.removeClass("scroll-scrollx_visible").find("div").andSelf().off(".scrollbar");
            this.scrolly.scrollbar.removeClass("scroll-scrolly_visible").find("div").andSelf().off(".scrollbar");

            this.wrapper.remove();

            $(doc).add("body").off(".scrollbar");

            if($.isFunction(this.options.onDestroy))
                this.options.onDestroy.apply(this, [this.container]);
        },



        getScrollbar: function(d){

            var scrollbar = this.options["scroll" + d];
            var html = {
                "advanced":
                '<div class="scroll-element_corner"></div>' +
                '<div class="scroll-arrow scroll-arrow_less"></div>' +
                '<div class="scroll-arrow scroll-arrow_more"></div>' +
                '<div class="scroll-element_outer">' +
                '    <div class="scroll-element_size"></div>' + // required! used for scrollbar size calculation !
                '    <div class="scroll-element_inner-wrapper">' +
                '        <div class="scroll-element_inner scroll-element_track">'  + // used for handling scrollbar click
                '            <div class="scroll-element_inner-bottom"></div>' +
                '        </div>' +
                '    </div>' +
                '    <div class="scroll-bar">' +
                '        <div class="scroll-bar_body">' +
                '            <div class="scroll-bar_body-inner"></div>' +
                '        </div>' +
                '        <div class="scroll-bar_bottom"></div>' +
                '        <div class="scroll-bar_center"></div>' +
                '    </div>' +
                '</div>',

                "simple":
                '<div class="scroll-element_outer">' +
                '    <div class="scroll-element_size"></div>'  + // required! used for scrollbar size calculation !
                '    <div class="scroll-element_track"></div>' + // used for handling scrollbar click
                '    <div class="scroll-bar"></div>' +
                '</div>'
            };
            var type = html[this.options.type] ? this.options.type : "advanced";

            if(scrollbar){
                if(typeof(scrollbar) == "string"){
                    scrollbar = $(scrollbar).appendTo(this.wrapper);
                } else {
                    scrollbar = $(scrollbar);
                }
            } else {
                scrollbar = $("<div>").addClass("scroll-element").html(html[type]).appendTo(this.wrapper);
            }

            if(this.options.showArrows){
                scrollbar.addClass("scroll-element_arrows_visible");
            }

            return scrollbar.addClass("scroll-" + d);
        },



        init: function(options){

            // init variables
            var S = this;

            var c = this.container;
            var cw = this.containerWrapper || c;
            var o = $.extend(this.options, options || {});
            var s = {
                "x": this.scrollx,
                "y": this.scrolly
            };
            var w = this.wrapper;

            var initScroll = {
                "scrollLeft": c.scrollLeft(),
                "scrollTop": c.scrollTop()
            };

            // do not init if in ignorable browser
            if ((browser.mobile && o.ignoreMobile) || (browser.overlay && o.ignoreOverlay)) {
                return false;
            }

            // init scroll container
            if(!w){
                this.wrapper = w = $('<div>').addClass('scroll-wrapper').addClass(c.attr('class'))
                .css('position', c.css('position') == 'absolute' ? 'absolute' : 'relative')
                .insertBefore(c).append(c);

                if(c.is('textarea')){
                    this.containerWrapper = cw = $('<div>').insertBefore(c).append(c);
                    w.addClass('scroll-textarea');

	                // Added for "disabled" support
	                if (c.prop('disabled')) {
		                w.addClass('disabled');
	                }
                }

                cw.addClass("scroll-content").css({
                    "height":"",
                    "margin-bottom": browser.scroll.height * -1 + px,
                    "margin-right":  browser.scroll.width  * -1 + px
                });

                c.on("scroll.scrollbar", function(event){
                    if($.isFunction(o.onScroll)){
                        o.onScroll.call(S, {
                            "maxScroll": s.y.maxScrollOffset,
                            "scroll": c.scrollTop(),
                            "size": s.y.size,
                            "visible": s.y.visible
                        }, {
                            "maxScroll": s.x.maxScrollOffset,
                            "scroll": c.scrollLeft(),
                            "size": s.x.size,
                            "visible": s.x.visible
                        });
                    }
                    s.x.isVisible && s.x.scroller.css("left", c.scrollLeft() * s.x.kx + px);
                    s.y.isVisible && s.y.scroller.css("top",  c.scrollTop()  * s.y.kx + px);
                });

                /* prevent native scrollbars to be visible on #anchor click */
                w.on("scroll", function(){
                    w.scrollTop(0).scrollLeft(0);
                });

                if(o.disableBodyScroll){
                    var handleMouseScroll = function(event){
                        isVerticalScroll(event) ?
                        s.y.isVisible && s.y.mousewheel(event) :
                        s.x.isVisible && s.x.mousewheel(event);
                    };
                    w.on({
                        "MozMousePixelScroll.scrollbar": handleMouseScroll,
                        "mousewheel.scrollbar": handleMouseScroll
                    });

                    if(browser.mobile){
                        w.on("touchstart.scrollbar", function(event){
                            var touch = event.originalEvent.touches && event.originalEvent.touches[0] || event;
                            var originalTouch = {
                                "pageX": touch.pageX,
                                "pageY": touch.pageY
                            };
                            var originalScroll = {
                                "left": c.scrollLeft(),
                                "top": c.scrollTop()
                            };
                            $(doc).on({
                                "touchmove.scrollbar": function(event){
                                    var touch = event.originalEvent.targetTouches && event.originalEvent.targetTouches[0] || event;
                                    c.scrollLeft(originalScroll.left + originalTouch.pageX - touch.pageX);
                                    c.scrollTop(originalScroll.top + originalTouch.pageY - touch.pageY);
                                    event.preventDefault();
                                },
                                "touchend.scrollbar": function(){
                                    $(doc).off(".scrollbar");
                                }
                            });
                        });
                    }
                }
                if($.isFunction(o.onInit))
                    o.onInit.apply(this, [c]);
            } else {
                cw.css({
                    "height":"",
                    "margin-bottom": browser.scroll.height * -1 + px,
                    "margin-right":  browser.scroll.width  * -1 + px
                });
            }

            // init scrollbars & recalculate sizes
            $.each(s, function(d, scrollx){

                var scrollCallback = null;
                var scrollForward = 1;
                var scrollOffset = (d == "x") ? "scrollLeft" : "scrollTop";
                var scrollStep = o.scrollStep;
                var scrollTo = function(){
                    var currentOffset = c[scrollOffset]();
                    c[scrollOffset](currentOffset + scrollStep);
                    if(scrollForward == 1 && (currentOffset + scrollStep) >= scrollToValue)
                        currentOffset = c[scrollOffset]();
                    if(scrollForward == -1 && (currentOffset + scrollStep) <= scrollToValue)
                        currentOffset = c[scrollOffset]();
                    if(c[scrollOffset]() == currentOffset && scrollCallback){
                        scrollCallback();
                    }
                }
                var scrollToValue = 0;

                if(!scrollx.scrollbar){

                    scrollx.scrollbar = S.getScrollbar(d);
                    scrollx.scroller = scrollx.scrollbar.find(".scroll-bar");

                    scrollx.mousewheel = function(event){

                        if(!scrollx.isVisible || (d == 'x' && isVerticalScroll(event))){
                            return true;
                        }
                        if(d == 'y' && !isVerticalScroll(event)){
                            s.x.mousewheel(event);
                            return true;
                        }

                        var delta = event.originalEvent.wheelDelta * -1 || event.originalEvent.detail;
                        var maxScrollValue = scrollx.size - scrollx.visible - scrollx.offset;

                        if(!((scrollToValue <= 0 && delta < 0) || (scrollToValue >= maxScrollValue && delta > 0))){
                            scrollToValue = scrollToValue + delta;
                            if(scrollToValue < 0)
                                scrollToValue = 0;
                            if(scrollToValue > maxScrollValue)
                                scrollToValue = maxScrollValue;

                            S.scrollTo = S.scrollTo || {};
                            S.scrollTo[scrollOffset] = scrollToValue;
                            setTimeout(function(){
                                if(S.scrollTo){
                                    c.stop().animate(S.scrollTo, 240, 'linear', function(){
                                        scrollToValue = c[scrollOffset]();
                                    });
                                    S.scrollTo = null;
                                }
                            }, 1);
                        }

                        event.preventDefault();
                        return false;
                    };

                    scrollx.scrollbar.on({
                        "MozMousePixelScroll.scrollbar": scrollx.mousewheel,
                        "mousewheel.scrollbar": scrollx.mousewheel,
                        "mouseenter.scrollbar": function(){
                            scrollToValue = c[scrollOffset]();
                        }
                    });

                    // handle arrows & scroll inner mousedown event
                    scrollx.scrollbar.find(".scroll-arrow, .scroll-element_track")
                    .on("mousedown.scrollbar", function(event){

                        if(event.which != lmb)
                            return true;

                        scrollForward = 1;

                        var data = {
                            "eventOffset": event[(d == "x") ? "pageX" : "pageY"],
                            "maxScrollValue": scrollx.size - scrollx.visible - scrollx.offset,
                            "scrollbarOffset": scrollx.scroller.offset()[(d == "x") ? "left" : "top"],
                            "scrollbarSize": scrollx.scroller[(d == "x") ? "outerWidth" : "outerHeight"]()
                        };
                        var timeout = 0, timer = 0;

                        if($(this).hasClass('scroll-arrow')){
                            scrollForward = $(this).hasClass("scroll-arrow_more") ? 1 : -1;
                            scrollStep = o.scrollStep * scrollForward;
                            scrollToValue = scrollForward > 0 ? data.maxScrollValue : 0;
                        } else {
                            scrollForward = (data.eventOffset > (data.scrollbarOffset + data.scrollbarSize) ? 1
                                : (data.eventOffset < data.scrollbarOffset ? -1 : 0));
                            scrollStep = Math.round(scrollx.visible * 0.75) * scrollForward;
                            scrollToValue = (data.eventOffset - data.scrollbarOffset -
                                (o.stepScrolling ? (scrollForward == 1 ? data.scrollbarSize : 0)
                                    : Math.round(data.scrollbarSize / 2)));
                            scrollToValue = c[scrollOffset]() + (scrollToValue / scrollx.kx);
                        }

                        S.scrollTo = S.scrollTo || {};
                        S.scrollTo[scrollOffset] = o.stepScrolling ? c[scrollOffset]() + scrollStep : scrollToValue;

                        if(o.stepScrolling){
                            scrollCallback = function(){
                                scrollToValue = c[scrollOffset]();
                                clearInterval(timer);
                                clearTimeout(timeout);
                                timeout = 0;
                                timer = 0;
                            };
                            timeout = setTimeout(function(){
                                timer = setInterval(scrollTo, 40);
                            }, o.duration + 100);
                        }

                        setTimeout(function(){
                            if(S.scrollTo){
                                c.animate(S.scrollTo, o.duration);
                                S.scrollTo = null;
                            }
                        }, 1);

                        return handleMouseDown(scrollCallback, event);
                    });

                    // handle scrollbar drag'n'drop
                    scrollx.scroller.on("mousedown.scrollbar", function(event){

                        if(event.which != lmb)
                            return true;

                        var eventPosition = event[(d == "x")? "pageX" : "pageY"];
                        var initOffset = c[scrollOffset]();

                        scrollx.scrollbar.addClass("scroll-draggable");

                        $(doc).on("mousemove.scrollbar", function(event){
                            var diff = parseInt((event[(d == "x")? "pageX" : "pageY"] - eventPosition) / scrollx.kx, 10);
                            c[scrollOffset](initOffset + diff);
                        });

                        return handleMouseDown(function(){
                            scrollx.scrollbar.removeClass("scroll-draggable");
                            scrollToValue = c[scrollOffset]();
                        }, event);
                    });
                }
            });

            // remove classes & reset applied styles
            $.each(s, function(d, scrollx){
                var scrollClass = "scroll-scroll" + d + "_visible";
                var scrolly = (d == "x") ? s.y : s.x;

                scrollx.scrollbar.removeClass(scrollClass);
                scrolly.scrollbar.removeClass(scrollClass);
                cw.removeClass(scrollClass);
            });

            // calculate init sizes
            $.each(s, function(d, scrollx){
                $.extend(scrollx, (d == "x") ? {
                    "offset": parseInt(c.css("left"), 10) || 0,
                    "size": c.prop("scrollWidth"),
                    "visible": w.width()
                } : {
                    "offset": parseInt(c.css("top"), 10) || 0,
                    "size": c.prop("scrollHeight"),
                    "visible": w.height()
                });
            });


            var updateScroll = function(d, scrollx){

                var scrollClass = "scroll-scroll" + d + "_visible";
                var scrolly = (d == "x") ? s.y : s.x;
                var offset = parseInt(c.css((d == "x") ? "left" : "top"), 10) || 0;

                var AreaSize = scrollx.size;
                var AreaVisible = scrollx.visible + offset;

                scrollx.isVisible = (AreaSize - AreaVisible) > 1; // bug in IE9/11 with 1px diff
                if(scrollx.isVisible){
                    scrollx.scrollbar.addClass(scrollClass);
                    scrolly.scrollbar.addClass(scrollClass);
                    cw.addClass(scrollClass);
                } else {
                    scrollx.scrollbar.removeClass(scrollClass);
                    scrolly.scrollbar.removeClass(scrollClass);
                    cw.removeClass(scrollClass);
                }

                if(d == "y" && (scrollx.isVisible || scrollx.size < scrollx.visible)){
                    cw.css("height", (AreaVisible + browser.scroll.height) + px);
                }

                if(s.x.size != c.prop("scrollWidth")
                    || s.y.size != c.prop("scrollHeight")
                    || s.x.visible != w.width()
                    || s.y.visible != w.height()
                    || s.x.offset  != (parseInt(c.css("left"), 10) || 0)
                    || s.y.offset  != (parseInt(c.css("top"), 10) || 0)
                    ){
                    $.each(s, function(d, scrollx){
                        $.extend(scrollx, (d == "x") ? {
                            "offset": parseInt(c.css("left"), 10) || 0,
                            "size": c.prop("scrollWidth"),
                            "visible": w.width()
                        } : {
                            "offset": parseInt(c.css("top"), 10) || 0,
                            "size": c.prop("scrollHeight"),
                            "visible": w.height()
                        });
                    });
                    updateScroll(d == "x" ? "y" : "x", scrolly);
                }
            };
            $.each(s, updateScroll);

            if($.isFunction(o.onUpdate))
                o.onUpdate.apply(this, [c]);

            // calculate scroll size
            $.each(s, function(d, scrollx){

                var cssOffset = (d == "x") ? "left" : "top";
                var cssFullSize = (d == "x") ? "outerWidth" : "outerHeight";
                var cssSize = (d == "x") ? "width" : "height";
                var offset = parseInt(c.css(cssOffset), 10) || 0;

                var AreaSize = scrollx.size;
                var AreaVisible = scrollx.visible + offset;

                var scrollSize = scrollx.scrollbar.find(".scroll-element_size");
                scrollSize = scrollSize[cssFullSize]() + (parseInt(scrollSize.css(cssOffset), 10) || 0);

                if(o.autoScrollSize){
                    scrollx.scrollbarSize = parseInt(scrollSize * AreaVisible / AreaSize, 10);
                    scrollx.scroller.css(cssSize, scrollx.scrollbarSize + px);
                }

                scrollx.scrollbarSize = scrollx.scroller[cssFullSize]();
                scrollx.kx = ((scrollSize - scrollx.scrollbarSize) / (AreaSize - AreaVisible)) || 1;
                scrollx.maxScrollOffset = AreaSize - AreaVisible;
            });

            c.scrollLeft(initScroll.scrollLeft).scrollTop(initScroll.scrollTop).trigger("scroll");
        }
    };

    /*
     * Extend jQuery as plugin
     *
     * @param {object|string} options or command to execute
     * @param {object|array} args additional arguments as array []
     */
    $.fn.scrollbar = function(options, args){

        var toReturn = this;

        if(options === "get")
            toReturn = null;

        this.each(function() {

            var container = $(this);

            if(container.hasClass("scroll-wrapper")
                || container.get(0).nodeName == "body"){
                return true;
            }

            var instance = container.data("scrollbar");
            if(instance){
                if(options === "get"){
                    toReturn = instance;
                    return false;
                }

                var func = (typeof options == "string" && instance[options]) ? options : "init";
                instance[func].apply(instance, $.isArray(args) ? args : []);

                if(options === "destroy"){
                    container.removeData("scrollbar");
                    while($.inArray(instance, browser.scrolls) >= 0)
                        browser.scrolls.splice($.inArray(instance, browser.scrolls), 1);
                }
            } else {
                if(typeof options != "string"){
                    instance = new customScrollbar(container, options);
                    container.data("scrollbar", instance);
                    browser.scrolls.push(instance);
                }
            }
            return true;
        });

        return toReturn;
    };

    /**
     * Connect default options to global object
     */
    $.fn.scrollbar.options = defaults;

    /**
     * Extend AngularJS as UI directive
     *
     *
     */
    if(win.angular){
        (function(angular){
            var app = angular.module('jQueryScrollbar', []);
            app.directive('jqueryScrollbar', function(){
                return {
                    "link": function(scope, element){
                        element.scrollbar(scope.options).on('$destroy', function(){
                            element.scrollbar('destroy');
                        });
                    },
                    "restring": "AC",
                    "scope": {
                        "options": "=jqueryScrollbar"
                    }
                };
            });
        })(win.angular);
    }

    /**
     * Check if scroll content/container size is changed
     */
    var timer = 0, timerCounter = 0;
    var updateScrollbars = function(force){
        var i, c, o, s, w, x, y;
        for( i = 0; i < browser.scrolls.length; i++){
            s = browser.scrolls[i];
            c = s.container;
            o = s.options;
            w = s.wrapper;
            x = s.scrollx;
            y = s.scrolly;
            if(force || (o.autoUpdate && w && w.is(":visible") &&
                (c.prop("scrollWidth") != x.size
                    || c.prop("scrollHeight") != y.size
                    || w.width()  != x.visible
                    || w.height() != y.visible
                    ))){
                s.init();

                if(debug){
                    browser.log({
                        "scrollHeight":  c.prop("scrollHeight") + ":" + s.scrolly.size,
                        "scrollWidth":   c.prop("scrollWidth") + ":" + s.scrollx.size,
                        "visibleHeight": w.height() + ":" + s.scrolly.visible,
                        "visibleWidth":  w.width() + ":" + s.scrollx.visible
                    }, true);
                    timerCounter++;
                }
            }
        }
        if(debug && timerCounter > 10){
            browser.log("Scroll updates exceed 10");
            updateScrollbars = function(){};
        } else {
            clearTimeout(timer);
            timer = setTimeout(updateScrollbars, 300);
        }
    };

    /* ADDITIONAL FUNCTIONS */
    /**
     * Get native browser scrollbar size (height/width)
     *
     * @param {Boolean} actual size or CSS size, default - CSS size
     * @returns {Object} with height, width
     */
    function getBrowserScrollSize(actualSize){

        if(browser.webkit && !actualSize){
            return {
                "height": 0,
                "width": 0
            };
        }

        if(!browser.data.outer){
            var css = {
                "border":  "none",
                "box-sizing": "content-box",
                "height":  "200px",
                "margin":  "0",
                "padding": "0",
                "width":   "200px"
            };
            browser.data.inner = $("<div>").css($.extend({}, css));
            browser.data.outer = $("<div>").css($.extend({
                "left":       "-1000px",
                "overflow":   "scroll",
                "position":   "absolute",
                "top":        "-1000px"
            }, css)).append(browser.data.inner).appendTo("body");
        }

        browser.data.outer.scrollLeft(1000).scrollTop(1000);

        return {
            "height": Math.ceil((browser.data.outer.offset().top - browser.data.inner.offset().top) || 0),
            "width": Math.ceil((browser.data.outer.offset().left - browser.data.inner.offset().left) || 0)
        };
    }

    function handleMouseDown(callback, event){
        $(doc).on({
            "blur.scrollbar": function(){
                $(doc).add('body').off('.scrollbar');
                callback && callback();
            },
            "dragstart.scrollbar": function(event){
                event.preventDefault();
                return false;
            },
            "mouseup.scrollbar": function(){
                $(doc).add('body').off('.scrollbar');
                callback && callback();
            }
        });
        $("body").on({
            "selectstart.scrollbar": function(event){
                event.preventDefault();
                return false;
            }
        });
        event && event.preventDefault();
        return false;
    }

    /**
     * Check if native browser scrollbars overlay content
     *
     * @returns {Boolean}
     */
    function isScrollOverlaysContent(){
        var scrollSize = getBrowserScrollSize(true);
        return !(scrollSize.height || scrollSize.width);
    }

    function isVerticalScroll(event){
        var e = event.originalEvent;
        if (e.axis && e.axis === e.HORIZONTAL_AXIS)
            return false;
        if (e.wheelDeltaX)
            return false;
        return true;
    }

})(jQuery, document, window);
// require "shards/nephrite/_nephrite.js"
/**
 * Swiper 3.2.5
 * Most modern mobile touch slider and framework with hardware accelerated transitions
 *
 * http://www.idangero.us/swiper/
 *
 * Copyright 2015, Vladimir Kharlampidi
 * The iDangero.us
 * http://www.idangero.us/
 *
 * Licensed under MIT
 *
 * Released on: November 21, 2015
 */
(function () {
	'use strict';
	var $;
	/*===========================
	 Swiper
	 ===========================*/
	var Swiper = function (container, params) {
		if (!(this instanceof Swiper)) return new Swiper(container, params);
		
		var defaults = {
			direction: 'horizontal',
			touchEventsTarget: 'container',
			initialSlide: 0,
			speed: 300,
			// autoplay
			autoplay: false,
			autoplayDisableOnInteraction: true,
			// To support iOS's swipe-to-go-back gesture (when being used in-app, with UIWebView).
			iOSEdgeSwipeDetection: false,
			iOSEdgeSwipeThreshold: 20,
			// Free mode
			freeMode: false,
			freeModeMomentum: true,
			freeModeMomentumRatio: 1,
			freeModeMomentumBounce: true,
			freeModeMomentumBounceRatio: 1,
			freeModeSticky: false,
			freeModeMinimumVelocity: 0.02,
			// Autoheight
			autoHeight: false,
			// Set wrapper width
			setWrapperSize: false,
			// Virtual Translate
			virtualTranslate: false,
			// Effects
			effect: 'slide', // 'slide' or 'fade' or 'cube' or 'coverflow'
			coverflow: {
				rotate: 50,
				stretch: 0,
				depth: 100,
				modifier: 1,
				slideShadows : true
			},
			cube: {
				slideShadows: true,
				shadow: true,
				shadowOffset: 20,
				shadowScale: 0.94
			},
			fade: {
				crossFade: false
			},
			// Parallax
			parallax: false,
			// Scrollbar
			scrollbar: null,
			scrollbarHide: true,
			scrollbarDraggable: false,
			scrollbarSnapOnRelease: false,
			// Keyboard Mousewheel
			keyboardControl: false,
			mousewheelControl: false,
			mousewheelReleaseOnEdges: false,
			mousewheelInvert: false,
			mousewheelForceToAxis: false,
			mousewheelSensitivity: 1,
			// Hash Navigation
			hashnav: false,
			// Breakpoints
			breakpoints: undefined,
			// Slides grid
			spaceBetween: 0,
			slidesPerView: 1,
			slidesPerColumn: 1,
			slidesPerColumnFill: 'column',
			slidesPerGroup: 1,
			centeredSlides: false,
			slidesOffsetBefore: 0, // in px
			slidesOffsetAfter: 0, // in px
			// Round length
			roundLengths: false,
			// Touches
			touchRatio: 1,
			touchAngle: 45,
			simulateTouch: true,
			shortSwipes: true,
			longSwipes: true,
			longSwipesRatio: 0.5,
			longSwipesMs: 300,
			followFinger: true,
			onlyExternal: false,
			threshold: 0,
			touchMoveStopPropagation: true,
			// Pagination
			pagination: null,
			paginationElement: 'span',
			paginationClickable: false,
			paginationHide: false,
			paginationBulletRender: null,
			// Resistance
			resistance: true,
			resistanceRatio: 0.85,
			// Next/prev buttons
			nextButton: null,
			prevButton: null,
			// Progress
			watchSlidesProgress: false,
			watchSlidesVisibility: false,
			// Cursor
			grabCursor: false,
			// Clicks
			preventClicks: true,
			preventClicksPropagation: true,
			slideToClickedSlide: false,
			// Lazy Loading
			lazyLoading: false,
			lazyLoadingInPrevNext: false,
			lazyLoadingOnTransitionStart: false,
			// Images
			preloadImages: true,
			updateOnImagesReady: true,
			// loop
			loop: false,
			loopAdditionalSlides: 0,
			loopedSlides: null,
			// Control
			control: undefined,
			controlInverse: false,
			controlBy: 'slide', //or 'container'
			// Swiping/no swiping
			allowSwipeToPrev: true,
			allowSwipeToNext: true,
			swipeHandler: null, //'.swipe-handler',
			noSwiping: true,
			noSwipingClass: 'swiper-no-swiping',
			// NS
			slideClass: 'swiper-slide',
			slideActiveClass: 'swiper-slide-active',
			slideVisibleClass: 'swiper-slide-visible',
			slideDuplicateClass: 'swiper-slide-duplicate',
			slideNextClass: 'swiper-slide-next',
			slidePrevClass: 'swiper-slide-prev',
			wrapperClass: 'swiper-wrapper',
			bulletClass: 'swiper-pagination-bullet',
			bulletActiveClass: 'swiper-pagination-bullet-active',
			buttonDisabledClass: 'swiper-button-disabled',
			paginationHiddenClass: 'swiper-pagination-hidden',
			// Observer
			observer: false,
			observeParents: false,
			// Accessibility
			a11y: false,
			prevSlideMessage: 'Previous slide',
			nextSlideMessage: 'Next slide',
			firstSlideMessage: 'This is the first slide',
			lastSlideMessage: 'This is the last slide',
			paginationBulletMessage: 'Go to slide {{index}}',
			// Callbacks
			runCallbacksOnInit: true
			/*
			 Callbacks:
			 onInit: function (swiper)
			 onDestroy: function (swiper)
			 onClick: function (swiper, e)
			 onTap: function (swiper, e)
			 onDoubleTap: function (swiper, e)
			 onSliderMove: function (swiper, e)
			 onSlideChangeStart: function (swiper)
			 onSlideChangeEnd: function (swiper)
			 onTransitionStart: function (swiper)
			 onTransitionEnd: function (swiper)
			 onImagesReady: function (swiper)
			 onProgress: function (swiper, progress)
			 onTouchStart: function (swiper, e)
			 onTouchMove: function (swiper, e)
			 onTouchMoveOpposite: function (swiper, e)
			 onTouchEnd: function (swiper, e)
			 onReachBeginning: function (swiper)
			 onReachEnd: function (swiper)
			 onSetTransition: function (swiper, duration)
			 onSetTranslate: function (swiper, translate)
			 onAutoplayStart: function (swiper)
			 onAutoplayStop: function (swiper),
			 onLazyImageLoad: function (swiper, slide, image)
			 onLazyImageReady: function (swiper, slide, image)
			 */
			
		};
		var initialVirtualTranslate = params && params.virtualTranslate;
		
		params = params || {};
		var originalParams = {};
		for (var param in params) {
			if (typeof params[param] === 'object') {
				originalParams[param] = {};
				for (var deepParam in params[param]) {
					originalParams[param][deepParam] = params[param][deepParam];
				}
			}
			else {
				originalParams[param] = params[param];
			}
		}
		for (var def in defaults) {
			if (typeof params[def] === 'undefined') {
				params[def] = defaults[def];
			}
			else if (typeof params[def] === 'object') {
				for (var deepDef in defaults[def]) {
					if (typeof params[def][deepDef] === 'undefined') {
						params[def][deepDef] = defaults[def][deepDef];
					}
				}
			}
		}
		
		// Swiper
		var s = this;
		
		// Params
		s.params = params;
		s.originalParams = originalParams;
		
		// Classname
		s.classNames = [];
		/*=========================
		 Dom Library and plugins
		 ===========================*/
		if (typeof $ !== 'undefined' && typeof Dom7 !== 'undefined'){
			$ = Dom7;
		}
		if (typeof $ === 'undefined') {
			if (typeof Dom7 === 'undefined') {
				$ = window.Dom7 || window.Zepto || window.jQuery;
			}
			else {
				$ = Dom7;
			}
			if (!$) return;
		}
		// Export it to Swiper instance
		s.$ = $;
		
		/*=========================
		 Breakpoints
		 ===========================*/
		s.currentBreakpoint = undefined;
		s.getActiveBreakpoint = function () {
			//Get breakpoint for window width
			if (!s.params.breakpoints) return false;
			var breakpoint = false;
			var points = [], point;
			for ( point in s.params.breakpoints ) {
				if (s.params.breakpoints.hasOwnProperty(point)) {
					points.push(point);
				}
			}
			points.sort(function (a, b) {
				return parseInt(a, 10) > parseInt(b, 10);
			});
			for (var i = 0; i < points.length; i++) {
				point = points[i];
				if (point >= window.innerWidth && !breakpoint) {
					breakpoint = point;
				}
			}
			return breakpoint || 'max';
		};
		s.setBreakpoint = function () {
			//Set breakpoint for window width and update parameters
			var breakpoint = s.getActiveBreakpoint();
			if (breakpoint && s.currentBreakpoint !== breakpoint) {
				var breakPointsParams = breakpoint in s.params.breakpoints ? s.params.breakpoints[breakpoint] : s.originalParams;
				for ( var param in breakPointsParams ) {
					s.params[param] = breakPointsParams[param];
				}
				s.currentBreakpoint = breakpoint;
			}
		};
		// Set breakpoint on load
		if (s.params.breakpoints) {
			s.setBreakpoint();
		}
		
		/*=========================
		 Preparation - Define Container, Wrapper and Pagination
		 ===========================*/
		s.container = $(container);
		if (s.container.length === 0) return;
		if (s.container.length > 1) {
			s.container.each(function () {
				new Swiper(this, params);
			});
			return;
		}
		
		// Save instance in container HTML Element and in data
		s.container[0].swiper = s;
		s.container.data('swiper', s);
		
		s.classNames.push('swiper-container-' + s.params.direction);
		
		if (s.params.freeMode) {
			s.classNames.push('swiper-container-free-mode');
		}
		if (!s.support.flexbox) {
			s.classNames.push('swiper-container-no-flexbox');
			s.params.slidesPerColumn = 1;
		}
		if (s.params.autoHeight) {
			s.classNames.push('swiper-container-autoheight');
		}
		// Enable slides progress when required
		if (s.params.parallax || s.params.watchSlidesVisibility) {
			s.params.watchSlidesProgress = true;
		}
		// Coverflow / 3D
		if (['cube', 'coverflow'].indexOf(s.params.effect) >= 0) {
			if (s.support.transforms3d) {
				s.params.watchSlidesProgress = true;
				s.classNames.push('swiper-container-3d');
			}
			else {
				s.params.effect = 'slide';
			}
		}
		if (s.params.effect !== 'slide') {
			s.classNames.push('swiper-container-' + s.params.effect);
		}
		if (s.params.effect === 'cube') {
			s.params.resistanceRatio = 0;
			s.params.slidesPerView = 1;
			s.params.slidesPerColumn = 1;
			s.params.slidesPerGroup = 1;
			s.params.centeredSlides = false;
			s.params.spaceBetween = 0;
			s.params.virtualTranslate = true;
			s.params.setWrapperSize = false;
		}
		if (s.params.effect === 'fade') {
			s.params.slidesPerView = 1;
			s.params.slidesPerColumn = 1;
			s.params.slidesPerGroup = 1;
			s.params.watchSlidesProgress = true;
			s.params.spaceBetween = 0;
			if (typeof initialVirtualTranslate === 'undefined') {
				s.params.virtualTranslate = true;
			}
		}
		
		// Grab Cursor
		if (s.params.grabCursor && s.support.touch) {
			s.params.grabCursor = false;
		}
		
		// Wrapper
		s.wrapper = s.container.children('.' + s.params.wrapperClass);
		
		// Pagination
		if (s.params.pagination) {
			s.paginationContainer = $(s.params.pagination);
			if (s.params.paginationClickable) {
				s.paginationContainer.addClass('swiper-pagination-clickable');
			}
		}
		
		// Is Horizontal
		function isH() {
			return s.params.direction === 'horizontal';
		}
		
		// RTL
		s.rtl = isH() && (s.container[0].dir.toLowerCase() === 'rtl' || s.container.css('direction') === 'rtl');
		if (s.rtl) {
			s.classNames.push('swiper-container-rtl');
		}
		
		// Wrong RTL support
		if (s.rtl) {
			s.wrongRTL = s.wrapper.css('display') === '-webkit-box';
		}
		
		// Columns
		if (s.params.slidesPerColumn > 1) {
			s.classNames.push('swiper-container-multirow');
		}
		
		// Check for Android
		if (s.device.android) {
			s.classNames.push('swiper-container-android');
		}
		
		// Add classes
		s.container.addClass(s.classNames.join(' '));
		
		// Translate
		s.translate = 0;
		
		// Progress
		s.progress = 0;
		
		// Velocity
		s.velocity = 0;
		
		/*=========================
		 Locks, unlocks
		 ===========================*/
		s.lockSwipeToNext = function () {
			s.params.allowSwipeToNext = false;
		};
		s.lockSwipeToPrev = function () {
			s.params.allowSwipeToPrev = false;
		};
		s.lockSwipes = function () {
			s.params.allowSwipeToNext = s.params.allowSwipeToPrev = false;
		};
		s.unlockSwipeToNext = function () {
			s.params.allowSwipeToNext = true;
		};
		s.unlockSwipeToPrev = function () {
			s.params.allowSwipeToPrev = true;
		};
		s.unlockSwipes = function () {
			s.params.allowSwipeToNext = s.params.allowSwipeToPrev = true;
		};
		
		/*=========================
		 Round helper
		 ===========================*/
		function round(a) {
			return Math.floor(a);
		}
		/*=========================
		 Set grab cursor
		 ===========================*/
		if (s.params.grabCursor) {
			s.container[0].style.cursor = 'move';
			s.container[0].style.cursor = '-webkit-grab';
			s.container[0].style.cursor = '-moz-grab';
			s.container[0].style.cursor = 'grab';
		}
		/*=========================
		 Update on Images Ready
		 ===========================*/
		s.imagesToLoad = [];
		s.imagesLoaded = 0;
		
		s.loadImage = function (imgElement, src, srcset, checkForComplete, callback) {
			var image;
			function onReady () {
				if (callback) callback();
			}
			if (!imgElement.complete || !checkForComplete) {
				if (src) {
					image = new window.Image();
					image.onload = onReady;
					image.onerror = onReady;
					if (srcset) {
						image.srcset = srcset;
					}
					if (src) {
						image.src = src;
					}
				} else {
					onReady();
				}
				
			} else {//image already loaded...
				onReady();
			}
		};
		s.preloadImages = function () {
			s.imagesToLoad = s.container.find('img');
			function _onReady() {
				if (typeof s === 'undefined' || s === null) return;
				if (s.imagesLoaded !== undefined) s.imagesLoaded++;
				if (s.imagesLoaded === s.imagesToLoad.length) {
					if (s.params.updateOnImagesReady) s.update();
					s.emit('onImagesReady', s);
				}
			}
			for (var i = 0; i < s.imagesToLoad.length; i++) {
				s.loadImage(s.imagesToLoad[i], (s.imagesToLoad[i].currentSrc || s.imagesToLoad[i].getAttribute('src')), (s.imagesToLoad[i].srcset || s.imagesToLoad[i].getAttribute('srcset')), true, _onReady);
			}
		};
		
		/*=========================
		 Autoplay
		 ===========================*/
		s.autoplayTimeoutId = undefined;
		s.autoplaying = false;
		s.autoplayPaused = false;
		function autoplay() {
			s.autoplayTimeoutId = setTimeout(function () {
				if (s.params.loop) {
					s.fixLoop();
					s._slideNext();
				}
				else {
					if (!s.isEnd) {
						s._slideNext();
					}
					else {
						if (!params.autoplayStopOnLast) {
							s._slideTo(0);
						}
						else {
							s.stopAutoplay();
						}
					}
				}
			}, s.params.autoplay);
		}
		s.startAutoplay = function () {
			if (typeof s.autoplayTimeoutId !== 'undefined') return false;
			if (!s.params.autoplay) return false;
			if (s.autoplaying) return false;
			s.autoplaying = true;
			s.emit('onAutoplayStart', s);
			autoplay();
		};
		s.stopAutoplay = function (internal) {
			if (!s.autoplayTimeoutId) return;
			if (s.autoplayTimeoutId) clearTimeout(s.autoplayTimeoutId);
			s.autoplaying = false;
			s.autoplayTimeoutId = undefined;
			s.emit('onAutoplayStop', s);
		};
		s.pauseAutoplay = function (speed) {
			if (s.autoplayPaused) return;
			if (s.autoplayTimeoutId) clearTimeout(s.autoplayTimeoutId);
			s.autoplayPaused = true;
			if (speed === 0) {
				s.autoplayPaused = false;
				autoplay();
			}
			else {
				s.wrapper.transitionEnd(function () {
					if (!s) return;
					s.autoplayPaused = false;
					if (!s.autoplaying) {
						s.stopAutoplay();
					}
					else {
						autoplay();
					}
				});
			}
		};
		/*=========================
		 Min/Max Translate
		 ===========================*/
		s.minTranslate = function () {
			return (-s.snapGrid[0]);
		};
		s.maxTranslate = function () {
			return (-s.snapGrid[s.snapGrid.length - 1]);
		};
		/*=========================
		 Slider/slides sizes
		 ===========================*/
		s.updateAutoHeight = function () {
			// Update Height
			var newHeight = s.slides.eq(s.activeIndex)[0].offsetHeight;
			if (newHeight) s.wrapper.css('height', s.slides.eq(s.activeIndex)[0].offsetHeight + 'px');
		};
		s.updateContainerSize = function () {
			var width, height;
			if (typeof s.params.width !== 'undefined') {
				width = s.params.width;
			}
			else {
				width = s.container[0].clientWidth;
			}
			if (typeof s.params.height !== 'undefined') {
				height = s.params.height;
			}
			else {
				height = s.container[0].clientHeight;
			}
			if (width === 0 && isH() || height === 0 && !isH()) {
				return;
			}
			
			//Subtract paddings
			width = width - parseInt(s.container.css('padding-left'), 10) - parseInt(s.container.css('padding-right'), 10);
			height = height - parseInt(s.container.css('padding-top'), 10) - parseInt(s.container.css('padding-bottom'), 10);
			
			// Store values
			s.width = width;
			s.height = height;
			s.size = isH() ? s.width : s.height;
		};
		
		s.updateSlidesSize = function () {
			s.slides = s.wrapper.children('.' + s.params.slideClass);
			s.snapGrid = [];
			s.slidesGrid = [];
			s.slidesSizesGrid = [];
			
			var spaceBetween = s.params.spaceBetween,
				slidePosition = -s.params.slidesOffsetBefore,
				i,
				prevSlideSize = 0,
				index = 0;
			if (typeof spaceBetween === 'string' && spaceBetween.indexOf('%') >= 0) {
				spaceBetween = parseFloat(spaceBetween.replace('%', '')) / 100 * s.size;
			}
			
			s.virtualSize = -spaceBetween;
			// reset margins
			if (s.rtl) s.slides.css({marginLeft: '', marginTop: ''});
			else s.slides.css({marginRight: '', marginBottom: ''});
			
			var slidesNumberEvenToRows;
			if (s.params.slidesPerColumn > 1) {
				if (Math.floor(s.slides.length / s.params.slidesPerColumn) === s.slides.length / s.params.slidesPerColumn) {
					slidesNumberEvenToRows = s.slides.length;
				}
				else {
					slidesNumberEvenToRows = Math.ceil(s.slides.length / s.params.slidesPerColumn) * s.params.slidesPerColumn;
				}
				if (s.params.slidesPerView !== 'auto' && s.params.slidesPerColumnFill === 'row') {
					slidesNumberEvenToRows = Math.max(slidesNumberEvenToRows, s.params.slidesPerView * s.params.slidesPerColumn);
				}
			}
			
			// Calc slides
			var slideSize;
			var slidesPerColumn = s.params.slidesPerColumn;
			var slidesPerRow = slidesNumberEvenToRows / slidesPerColumn;
			var numFullColumns = slidesPerRow - (s.params.slidesPerColumn * slidesPerRow - s.slides.length);
			for (i = 0; i < s.slides.length; i++) {
				slideSize = 0;
				var slide = s.slides.eq(i);
				if (s.params.slidesPerColumn > 1) {
					// Set slides order
					var newSlideOrderIndex;
					var column, row;
					if (s.params.slidesPerColumnFill === 'column') {
						column = Math.floor(i / slidesPerColumn);
						row = i - column * slidesPerColumn;
						if (column > numFullColumns || (column === numFullColumns && row === slidesPerColumn-1)) {
							if (++row >= slidesPerColumn) {
								row = 0;
								column++;
							}
						}
						newSlideOrderIndex = column + row * slidesNumberEvenToRows / slidesPerColumn;
						slide
							.css({
								'-webkit-box-ordinal-group': newSlideOrderIndex,
								'-moz-box-ordinal-group': newSlideOrderIndex,
								'-ms-flex-order': newSlideOrderIndex,
								'-webkit-order': newSlideOrderIndex,
								'order': newSlideOrderIndex
							});
					}
					else {
						row = Math.floor(i / slidesPerRow);
						column = i - row * slidesPerRow;
					}
					slide
						.css({
							'margin-top': (row !== 0 && s.params.spaceBetween) && (s.params.spaceBetween + 'px')
						})
						.attr('data-swiper-column', column)
						.attr('data-swiper-row', row);
					
				}
				if (slide.css('display') === 'none') continue;
				if (s.params.slidesPerView === 'auto') {
					slideSize = isH() ? slide.outerWidth(true) : slide.outerHeight(true);
					if (s.params.roundLengths) slideSize = round(slideSize);
				}
				else {
					slideSize = (s.size - (s.params.slidesPerView - 1) * spaceBetween) / s.params.slidesPerView;
					if (s.params.roundLengths) slideSize = round(slideSize);
					
					if (isH()) {
						s.slides[i].style.width = slideSize + 'px';
					}
					else {
						s.slides[i].style.height = slideSize + 'px';
					}
				}
				s.slides[i].swiperSlideSize = slideSize;
				s.slidesSizesGrid.push(slideSize);
				
				
				if (s.params.centeredSlides) {
					slidePosition = slidePosition + slideSize / 2 + prevSlideSize / 2 + spaceBetween;
					if (i === 0) slidePosition = slidePosition - s.size / 2 - spaceBetween;
					if (Math.abs(slidePosition) < 1 / 1000) slidePosition = 0;
					if ((index) % s.params.slidesPerGroup === 0) s.snapGrid.push(slidePosition);
					s.slidesGrid.push(slidePosition);
				}
				else {
					if ((index) % s.params.slidesPerGroup === 0) s.snapGrid.push(slidePosition);
					s.slidesGrid.push(slidePosition);
					slidePosition = slidePosition + slideSize + spaceBetween;
				}
				
				s.virtualSize += slideSize + spaceBetween;
				
				prevSlideSize = slideSize;
				
				index ++;
			}
			s.virtualSize = Math.max(s.virtualSize, s.size) + s.params.slidesOffsetAfter;
			var newSlidesGrid;
			
			if (
				s.rtl && s.wrongRTL && (s.params.effect === 'slide' || s.params.effect === 'coverflow')) {
				s.wrapper.css({width: s.virtualSize + s.params.spaceBetween + 'px'});
			}
			if (!s.support.flexbox || s.params.setWrapperSize) {
				if (isH()) s.wrapper.css({width: s.virtualSize + s.params.spaceBetween + 'px'});
				else s.wrapper.css({height: s.virtualSize + s.params.spaceBetween + 'px'});
			}
			
			if (s.params.slidesPerColumn > 1) {
				s.virtualSize = (slideSize + s.params.spaceBetween) * slidesNumberEvenToRows;
				s.virtualSize = Math.ceil(s.virtualSize / s.params.slidesPerColumn) - s.params.spaceBetween;
				s.wrapper.css({width: s.virtualSize + s.params.spaceBetween + 'px'});
				if (s.params.centeredSlides) {
					newSlidesGrid = [];
					for (i = 0; i < s.snapGrid.length; i++) {
						if (s.snapGrid[i] < s.virtualSize + s.snapGrid[0]) newSlidesGrid.push(s.snapGrid[i]);
					}
					s.snapGrid = newSlidesGrid;
				}
			}
			
			// Remove last grid elements depending on width
			if (!s.params.centeredSlides) {
				newSlidesGrid = [];
				for (i = 0; i < s.snapGrid.length; i++) {
					if (s.snapGrid[i] <= s.virtualSize - s.size) {
						newSlidesGrid.push(s.snapGrid[i]);
					}
				}
				s.snapGrid = newSlidesGrid;
				if (Math.floor(s.virtualSize - s.size) > Math.floor(s.snapGrid[s.snapGrid.length - 1])) {
					s.snapGrid.push(s.virtualSize - s.size);
				}
			}
			if (s.snapGrid.length === 0) s.snapGrid = [0];
			
			if (s.params.spaceBetween !== 0) {
				if (isH()) {
					if (s.rtl) s.slides.css({marginLeft: spaceBetween + 'px'});
					else s.slides.css({marginRight: spaceBetween + 'px'});
				}
				else s.slides.css({marginBottom: spaceBetween + 'px'});
			}
			if (s.params.watchSlidesProgress) {
				s.updateSlidesOffset();
			}
		};
		s.updateSlidesOffset = function () {
			for (var i = 0; i < s.slides.length; i++) {
				s.slides[i].swiperSlideOffset = isH() ? s.slides[i].offsetLeft : s.slides[i].offsetTop;
			}
		};
		
		/*=========================
		 Slider/slides progress
		 ===========================*/
		s.updateSlidesProgress = function (translate) {
			if (typeof translate === 'undefined') {
				translate = s.translate || 0;
			}
			if (s.slides.length === 0) return;
			if (typeof s.slides[0].swiperSlideOffset === 'undefined') s.updateSlidesOffset();
			
			var offsetCenter = -translate;
			if (s.rtl) offsetCenter = translate;
			
			// Visible Slides
			s.slides.removeClass(s.params.slideVisibleClass);
			for (var i = 0; i < s.slides.length; i++) {
				var slide = s.slides[i];
				var slideProgress = (offsetCenter - slide.swiperSlideOffset) / (slide.swiperSlideSize + s.params.spaceBetween);
				if (s.params.watchSlidesVisibility) {
					var slideBefore = -(offsetCenter - slide.swiperSlideOffset);
					var slideAfter = slideBefore + s.slidesSizesGrid[i];
					var isVisible =
						(slideBefore >= 0 && slideBefore < s.size) ||
						(slideAfter > 0 && slideAfter <= s.size) ||
						(slideBefore <= 0 && slideAfter >= s.size);
					if (isVisible) {
						s.slides.eq(i).addClass(s.params.slideVisibleClass);
					}
				}
				slide.progress = s.rtl ? -slideProgress : slideProgress;
			}
		};
		s.updateProgress = function (translate) {
			if (typeof translate === 'undefined') {
				translate = s.translate || 0;
			}
			var translatesDiff = s.maxTranslate() - s.minTranslate();
			var wasBeginning = s.isBeginning;
			var wasEnd = s.isEnd;
			if (translatesDiff === 0) {
				s.progress = 0;
				s.isBeginning = s.isEnd = true;
			}
			else {
				s.progress = (translate - s.minTranslate()) / (translatesDiff);
				s.isBeginning = s.progress <= 0;
				s.isEnd = s.progress >= 1;
			}
			if (s.isBeginning && !wasBeginning) s.emit('onReachBeginning', s);
			if (s.isEnd && !wasEnd) s.emit('onReachEnd', s);
			
			if (s.params.watchSlidesProgress) s.updateSlidesProgress(translate);
			s.emit('onProgress', s, s.progress);
		};
		s.updateActiveIndex = function () {
			var translate = s.rtl ? s.translate : -s.translate;
			var newActiveIndex, i, snapIndex;
			for (i = 0; i < s.slidesGrid.length; i ++) {
				if (typeof s.slidesGrid[i + 1] !== 'undefined') {
					if (translate >= s.slidesGrid[i] && translate < s.slidesGrid[i + 1] - (s.slidesGrid[i + 1] - s.slidesGrid[i]) / 2) {
						newActiveIndex = i;
					}
					else if (translate >= s.slidesGrid[i] && translate < s.slidesGrid[i + 1]) {
						newActiveIndex = i + 1;
					}
				}
				else {
					if (translate >= s.slidesGrid[i]) {
						newActiveIndex = i;
					}
				}
			}
			// Normalize slideIndex
			if (newActiveIndex < 0 || typeof newActiveIndex === 'undefined') newActiveIndex = 0;
			// for (i = 0; i < s.slidesGrid.length; i++) {
			// if (- translate >= s.slidesGrid[i]) {
			// newActiveIndex = i;
			// }
			// }
			snapIndex = Math.floor(newActiveIndex / s.params.slidesPerGroup);
			if (snapIndex >= s.snapGrid.length) snapIndex = s.snapGrid.length - 1;
			
			if (newActiveIndex === s.activeIndex) {
				return;
			}
			s.snapIndex = snapIndex;
			s.previousIndex = s.activeIndex;
			s.activeIndex = newActiveIndex;
			s.updateClasses();
		};
		
		/*=========================
		 Classes
		 ===========================*/
		s.updateClasses = function () {
			s.slides.removeClass(s.params.slideActiveClass + ' ' + s.params.slideNextClass + ' ' + s.params.slidePrevClass);
			var activeSlide = s.slides.eq(s.activeIndex);
			// Active classes
			activeSlide.addClass(s.params.slideActiveClass);
			activeSlide.next('.' + s.params.slideClass).addClass(s.params.slideNextClass);
			activeSlide.prev('.' + s.params.slideClass).addClass(s.params.slidePrevClass);
			
			// Pagination
			if (s.bullets && s.bullets.length > 0) {
				s.bullets.removeClass(s.params.bulletActiveClass);
				var bulletIndex;
				if (s.params.loop) {
					bulletIndex = Math.ceil(s.activeIndex - s.loopedSlides)/s.params.slidesPerGroup;
					if (bulletIndex > s.slides.length - 1 - s.loopedSlides * 2) {
						bulletIndex = bulletIndex - (s.slides.length - s.loopedSlides * 2);
					}
					if (bulletIndex > s.bullets.length - 1) bulletIndex = bulletIndex - s.bullets.length;
				}
				else {
					if (typeof s.snapIndex !== 'undefined') {
						bulletIndex = s.snapIndex;
					}
					else {
						bulletIndex = s.activeIndex || 0;
					}
				}
				if (s.paginationContainer.length > 1) {
					s.bullets.each(function () {
						if ($(this).index() === bulletIndex) $(this).addClass(s.params.bulletActiveClass);
					});
				}
				else {
					s.bullets.eq(bulletIndex).addClass(s.params.bulletActiveClass);
				}
			}
			
			// Next/active buttons
			if (!s.params.loop) {
				if (s.params.prevButton) {
					if (s.isBeginning) {
						$(s.params.prevButton).addClass(s.params.buttonDisabledClass);
						if (s.params.a11y && s.a11y) s.a11y.disable($(s.params.prevButton));
					}
					else {
						$(s.params.prevButton).removeClass(s.params.buttonDisabledClass);
						if (s.params.a11y && s.a11y) s.a11y.enable($(s.params.prevButton));
					}
				}
				if (s.params.nextButton) {
					if (s.isEnd) {
						$(s.params.nextButton).addClass(s.params.buttonDisabledClass);
						if (s.params.a11y && s.a11y) s.a11y.disable($(s.params.nextButton));
					}
					else {
						$(s.params.nextButton).removeClass(s.params.buttonDisabledClass);
						if (s.params.a11y && s.a11y) s.a11y.enable($(s.params.nextButton));
					}
				}
			}
		};
		
		/*=========================
		 Pagination
		 ===========================*/
		s.updatePagination = function () {
			if (!s.params.pagination) return;
			if (s.paginationContainer && s.paginationContainer.length > 0) {
				var bulletsHTML = '';
				var numberOfBullets = s.params.loop ? Math.ceil((s.slides.length - s.loopedSlides * 2) / s.params.slidesPerGroup) : s.snapGrid.length;
				for (var i = 0; i < numberOfBullets; i++) {
					if (s.params.paginationBulletRender) {
						bulletsHTML += s.params.paginationBulletRender(i, s.params.bulletClass);
					}
					else {
						bulletsHTML += '<' + s.params.paginationElement+' class="' + s.params.bulletClass + '"></' + s.params.paginationElement + '>';
					}
				}
				s.paginationContainer.html(bulletsHTML);
				s.bullets = s.paginationContainer.find('.' + s.params.bulletClass);
				if (s.params.paginationClickable && s.params.a11y && s.a11y) {
					s.a11y.initPagination();
				}
			}
		};
		/*=========================
		 Common update method
		 ===========================*/
		s.update = function (updateTranslate) {
			s.updateContainerSize();
			s.updateSlidesSize();
			s.updateProgress();
			s.updatePagination();
			s.updateClasses();
			if (s.params.scrollbar && s.scrollbar) {
				s.scrollbar.set();
			}
			function forceSetTranslate() {
				newTranslate = Math.min(Math.max(s.translate, s.maxTranslate()), s.minTranslate());
				s.setWrapperTranslate(newTranslate);
				s.updateActiveIndex();
				s.updateClasses();
			}
			if (updateTranslate) {
				var translated, newTranslate;
				if (s.controller && s.controller.spline) {
					s.controller.spline = undefined;
				}
				if (s.params.freeMode) {
					forceSetTranslate();
					if (s.params.autoHeight) {
						s.updateAutoHeight();
					}
				}
				else {
					if ((s.params.slidesPerView === 'auto' || s.params.slidesPerView > 1) && s.isEnd && !s.params.centeredSlides) {
						translated = s.slideTo(s.slides.length - 1, 0, false, true);
					}
					else {
						translated = s.slideTo(s.activeIndex, 0, false, true);
					}
					if (!translated) {
						forceSetTranslate();
					}
				}
			}
			else if (s.params.autoHeight) {
				s.updateAutoHeight();
			}
		};
		
		/*=========================
		 Resize Handler
		 ===========================*/
		s.onResize = function (forceUpdatePagination) {
			//Breakpoints
			if (s.params.breakpoints) {
				s.setBreakpoint();
			}
			
			// Disable locks on resize
			var allowSwipeToPrev = s.params.allowSwipeToPrev;
			var allowSwipeToNext = s.params.allowSwipeToNext;
			s.params.allowSwipeToPrev = s.params.allowSwipeToNext = true;
			
			s.updateContainerSize();
			s.updateSlidesSize();
			if (s.params.slidesPerView === 'auto' || s.params.freeMode || forceUpdatePagination) s.updatePagination();
			if (s.params.scrollbar && s.scrollbar) {
				s.scrollbar.set();
			}
			if (s.controller && s.controller.spline) {
				s.controller.spline = undefined;
			}
			if (s.params.freeMode) {
				var newTranslate = Math.min(Math.max(s.translate, s.maxTranslate()), s.minTranslate());
				s.setWrapperTranslate(newTranslate);
				s.updateActiveIndex();
				s.updateClasses();
				
				if (s.params.autoHeight) {
					s.updateAutoHeight();
				}
			}
			else {
				s.updateClasses();
				if ((s.params.slidesPerView === 'auto' || s.params.slidesPerView > 1) && s.isEnd && !s.params.centeredSlides) {
					s.slideTo(s.slides.length - 1, 0, false, true);
				}
				else {
					s.slideTo(s.activeIndex, 0, false, true);
				}
			}
			// Return locks after resize
			s.params.allowSwipeToPrev = allowSwipeToPrev;
			s.params.allowSwipeToNext = allowSwipeToNext;
		};
		
		/*=========================
		 Events
		 ===========================*/
		
		//Define Touch Events
		var desktopEvents = ['mousedown', 'mousemove', 'mouseup'];
		if (window.navigator.pointerEnabled) desktopEvents = ['pointerdown', 'pointermove', 'pointerup'];
		else if (window.navigator.msPointerEnabled) desktopEvents = ['MSPointerDown', 'MSPointerMove', 'MSPointerUp'];
		s.touchEvents = {
			start : s.support.touch || !s.params.simulateTouch  ? 'touchstart' : desktopEvents[0],
			move : s.support.touch || !s.params.simulateTouch ? 'touchmove' : desktopEvents[1],
			end : s.support.touch || !s.params.simulateTouch ? 'touchend' : desktopEvents[2]
		};
		
		
		// WP8 Touch Events Fix
		if (window.navigator.pointerEnabled || window.navigator.msPointerEnabled) {
			(s.params.touchEventsTarget === 'container' ? s.container : s.wrapper).addClass('swiper-wp8-' + s.params.direction);
		}
		
		// Attach/detach events
		s.initEvents = function (detach) {
			var actionDom = detach ? 'off' : 'on';
			var action = detach ? 'removeEventListener' : 'addEventListener';
			var touchEventsTarget = s.params.touchEventsTarget === 'container' ? s.container[0] : s.wrapper[0];
			var target = s.support.touch ? touchEventsTarget : document;
			
			var moveCapture = s.params.nested ? true : false;
			
			//Touch Events
			if (s.browser.ie) {
				touchEventsTarget[action](s.touchEvents.start, s.onTouchStart, false);
				target[action](s.touchEvents.move, s.onTouchMove, moveCapture);
				target[action](s.touchEvents.end, s.onTouchEnd, false);
			}
			else {
				if (s.support.touch) {
					touchEventsTarget[action](s.touchEvents.start, s.onTouchStart, false);
					touchEventsTarget[action](s.touchEvents.move, s.onTouchMove, moveCapture);
					touchEventsTarget[action](s.touchEvents.end, s.onTouchEnd, false);
				}
				if (params.simulateTouch && !s.device.ios && !s.device.android) {
					touchEventsTarget[action]('mousedown', s.onTouchStart, false);
					document[action]('mousemove', s.onTouchMove, moveCapture);
					document[action]('mouseup', s.onTouchEnd, false);
				}
			}
			window[action]('resize', s.onResize);
			
			// Next, Prev, Index
			if (s.params.nextButton) {
				$(s.params.nextButton)[actionDom]('click', s.onClickNext);
				if (s.params.a11y && s.a11y) $(s.params.nextButton)[actionDom]('keydown', s.a11y.onEnterKey);
			}
			if (s.params.prevButton) {
				$(s.params.prevButton)[actionDom]('click', s.onClickPrev);
				if (s.params.a11y && s.a11y) $(s.params.prevButton)[actionDom]('keydown', s.a11y.onEnterKey);
			}
			if (s.params.pagination && s.params.paginationClickable) {
				$(s.paginationContainer)[actionDom]('click', '.' + s.params.bulletClass, s.onClickIndex);
				if (s.params.a11y && s.a11y) $(s.paginationContainer)[actionDom]('keydown', '.' + s.params.bulletClass, s.a11y.onEnterKey);
			}
			
			// Prevent Links Clicks
			if (s.params.preventClicks || s.params.preventClicksPropagation) touchEventsTarget[action]('click', s.preventClicks, true);
		};
		s.attachEvents = function (detach) {
			s.initEvents();
		};
		s.detachEvents = function () {
			s.initEvents(true);
		};
		
		/*=========================
		 Handle Clicks
		 ===========================*/
		// Prevent Clicks
		s.allowClick = true;
		s.preventClicks = function (e) {
			if (!s.allowClick) {
				if (s.params.preventClicks) e.preventDefault();
				if (s.params.preventClicksPropagation && s.animating) {
					e.stopPropagation();
					e.stopImmediatePropagation();
				}
			}
		};
		// Clicks
		s.onClickNext = function (e) {
			e.preventDefault();
			if (s.isEnd && !s.params.loop) return;
			s.slideNext();
		};
		s.onClickPrev = function (e) {
			e.preventDefault();
			if (s.isBeginning && !s.params.loop) return;
			s.slidePrev();
		};
		s.onClickIndex = function (e) {
			e.preventDefault();
			var index = $(this).index() * s.params.slidesPerGroup;
			if (s.params.loop) index = index + s.loopedSlides;
			s.slideTo(index);
		};
		
		/*=========================
		 Handle Touches
		 ===========================*/
		function findElementInEvent(e, selector) {
			var el = $(e.target);
			if (!el.is(selector)) {
				if (typeof selector === 'string') {
					el = el.parents(selector);
				}
				else if (selector.nodeType) {
					var found;
					el.parents().each(function (index, _el) {
						if (_el === selector) found = selector;
					});
					if (!found) return undefined;
					else return selector;
				}
			}
			if (el.length === 0) {
				return undefined;
			}
			return el[0];
		}
		s.updateClickedSlide = function (e) {
			var slide = findElementInEvent(e, '.' + s.params.slideClass);
			var slideFound = false;
			if (slide) {
				for (var i = 0; i < s.slides.length; i++) {
					if (s.slides[i] === slide) slideFound = true;
				}
			}
			
			if (slide && slideFound) {
				s.clickedSlide = slide;
				s.clickedIndex = $(slide).index();
			}
			else {
				s.clickedSlide = undefined;
				s.clickedIndex = undefined;
				return;
			}
			if (s.params.slideToClickedSlide && s.clickedIndex !== undefined && s.clickedIndex !== s.activeIndex) {
				var slideToIndex = s.clickedIndex,
					realIndex,
					duplicatedSlides;
				if (s.params.loop) {
					if (s.animating) return;
					realIndex = $(s.clickedSlide).attr('data-swiper-slide-index');
					if (s.params.centeredSlides) {
						if ((slideToIndex < s.loopedSlides - s.params.slidesPerView/2) || (slideToIndex > s.slides.length - s.loopedSlides + s.params.slidesPerView/2)) {
							s.fixLoop();
							slideToIndex = s.wrapper.children('.' + s.params.slideClass + '[data-swiper-slide-index="' + realIndex + '"]:not(.swiper-slide-duplicate)').eq(0).index();
							setTimeout(function () {
								s.slideTo(slideToIndex);
							}, 0);
						}
						else {
							s.slideTo(slideToIndex);
						}
					}
					else {
						if (slideToIndex > s.slides.length - s.params.slidesPerView) {
							s.fixLoop();
							slideToIndex = s.wrapper.children('.' + s.params.slideClass + '[data-swiper-slide-index="' + realIndex + '"]:not(.swiper-slide-duplicate)').eq(0).index();
							setTimeout(function () {
								s.slideTo(slideToIndex);
							}, 0);
						}
						else {
							s.slideTo(slideToIndex);
						}
					}
				}
				else {
					s.slideTo(slideToIndex);
				}
			}
		};
		
		var isTouched,
			isMoved,
			allowTouchCallbacks,
			touchStartTime,
			isScrolling,
			currentTranslate,
			startTranslate,
			allowThresholdMove,
		// Form elements to match
			formElements = 'input, select, textarea, button',
		// Last click time
			lastClickTime = Date.now(), clickTimeout,
		//Velocities
			velocities = [],
			allowMomentumBounce;
		
		// Animating Flag
		s.animating = false;
		
		// Touches information
		s.touches = {
			startX: 0,
			startY: 0,
			currentX: 0,
			currentY: 0,
			diff: 0
		};
		
		// Touch handlers
		var isTouchEvent, startMoving;
		s.onTouchStart = function (e) {
			if (e.originalEvent) e = e.originalEvent;
			isTouchEvent = e.type === 'touchstart';
			if (!isTouchEvent && 'which' in e && e.which === 3) return;
			if (s.params.noSwiping && findElementInEvent(e, '.' + s.params.noSwipingClass)) {
				s.allowClick = true;
				return;
			}
			if (s.params.swipeHandler) {
				if (!findElementInEvent(e, s.params.swipeHandler)) return;
			}
			
			var startX = s.touches.currentX = e.type === 'touchstart' ? e.targetTouches[0].pageX : e.pageX;
			var startY = s.touches.currentY = e.type === 'touchstart' ? e.targetTouches[0].pageY : e.pageY;
			
			// Do NOT start if iOS edge swipe is detected. Otherwise iOS app (UIWebView) cannot swipe-to-go-back anymore
			if(s.device.ios && s.params.iOSEdgeSwipeDetection && startX <= s.params.iOSEdgeSwipeThreshold) {
				return;
			}
			
			isTouched = true;
			isMoved = false;
			allowTouchCallbacks = true;
			isScrolling = undefined;
			startMoving = undefined;
			s.touches.startX = startX;
			s.touches.startY = startY;
			touchStartTime = Date.now();
			s.allowClick = true;
			s.updateContainerSize();
			s.swipeDirection = undefined;
			if (s.params.threshold > 0) allowThresholdMove = false;
			if (e.type !== 'touchstart') {
				var preventDefault = true;
				if ($(e.target).is(formElements)) preventDefault = false;
				if (document.activeElement && $(document.activeElement).is(formElements)) {
					document.activeElement.blur();
				}
				if (preventDefault) {
					e.preventDefault();
				}
			}
			s.emit('onTouchStart', s, e);
		};
		
		s.onTouchMove = function (e) {
			if (e.originalEvent) e = e.originalEvent;
			if (isTouchEvent && e.type === 'mousemove') return;
			if (e.preventedByNestedSwiper) return;
			if (s.params.onlyExternal) {
				// isMoved = true;
				s.allowClick = false;
				if (isTouched) {
					s.touches.startX = s.touches.currentX = e.type === 'touchmove' ? e.targetTouches[0].pageX : e.pageX;
					s.touches.startY = s.touches.currentY = e.type === 'touchmove' ? e.targetTouches[0].pageY : e.pageY;
					touchStartTime = Date.now();
				}
				return;
			}
			if (isTouchEvent && document.activeElement) {
				if (e.target === document.activeElement && $(e.target).is(formElements)) {
					isMoved = true;
					s.allowClick = false;
					return;
				}
			}
			if (allowTouchCallbacks) {
				s.emit('onTouchMove', s, e);
			}
			if (e.targetTouches && e.targetTouches.length > 1) return;
			
			s.touches.currentX = e.type === 'touchmove' ? e.targetTouches[0].pageX : e.pageX;
			s.touches.currentY = e.type === 'touchmove' ? e.targetTouches[0].pageY : e.pageY;
			
			if (typeof isScrolling === 'undefined') {
				var touchAngle = Math.atan2(Math.abs(s.touches.currentY - s.touches.startY), Math.abs(s.touches.currentX - s.touches.startX)) * 180 / Math.PI;
				isScrolling = isH() ? touchAngle > s.params.touchAngle : (90 - touchAngle > s.params.touchAngle);
			}
			if (isScrolling) {
				s.emit('onTouchMoveOpposite', s, e);
			}
			if (typeof startMoving === 'undefined' && s.browser.ieTouch) {
				if (s.touches.currentX !== s.touches.startX || s.touches.currentY !== s.touches.startY) {
					startMoving = true;
				}
			}
			if (!isTouched) return;
			if (isScrolling)  {
				isTouched = false;
				return;
			}
			if (!startMoving && s.browser.ieTouch) {
				return;
			}
			s.allowClick = false;
			s.emit('onSliderMove', s, e);
			e.preventDefault();
			if (s.params.touchMoveStopPropagation && !s.params.nested) {
				e.stopPropagation();
			}
			
			if (!isMoved) {
				if (params.loop) {
					s.fixLoop();
				}
				startTranslate = s.getWrapperTranslate();
				s.setWrapperTransition(0);
				if (s.animating) {
					s.wrapper.trigger('webkitTransitionEnd transitionend oTransitionEnd MSTransitionEnd msTransitionEnd');
				}
				if (s.params.autoplay && s.autoplaying) {
					if (s.params.autoplayDisableOnInteraction) {
						s.stopAutoplay();
					}
					else {
						s.pauseAutoplay();
					}
				}
				allowMomentumBounce = false;
				//Grab Cursor
				if (s.params.grabCursor) {
					s.container[0].style.cursor = 'move';
					s.container[0].style.cursor = '-webkit-grabbing';
					s.container[0].style.cursor = '-moz-grabbin';
					s.container[0].style.cursor = 'grabbing';
				}
			}
			isMoved = true;
			
			var diff = s.touches.diff = isH() ? s.touches.currentX - s.touches.startX : s.touches.currentY - s.touches.startY;
			
			diff = diff * s.params.touchRatio;
			if (s.rtl) diff = -diff;
			
			s.swipeDirection = diff > 0 ? 'prev' : 'next';
			currentTranslate = diff + startTranslate;
			
			var disableParentSwiper = true;
			if ((diff > 0 && currentTranslate > s.minTranslate())) {
				disableParentSwiper = false;
				if (s.params.resistance) currentTranslate = s.minTranslate() - 1 + Math.pow(-s.minTranslate() + startTranslate + diff, s.params.resistanceRatio);
			}
			else if (diff < 0 && currentTranslate < s.maxTranslate()) {
				disableParentSwiper = false;
				if (s.params.resistance) currentTranslate = s.maxTranslate() + 1 - Math.pow(s.maxTranslate() - startTranslate - diff, s.params.resistanceRatio);
			}
			
			if (disableParentSwiper) {
				e.preventedByNestedSwiper = true;
			}
			
			// Directions locks
			if (!s.params.allowSwipeToNext && s.swipeDirection === 'next' && currentTranslate < startTranslate) {
				currentTranslate = startTranslate;
			}
			if (!s.params.allowSwipeToPrev && s.swipeDirection === 'prev' && currentTranslate > startTranslate) {
				currentTranslate = startTranslate;
			}
			
			if (!s.params.followFinger) return;
			
			// Threshold
			if (s.params.threshold > 0) {
				if (Math.abs(diff) > s.params.threshold || allowThresholdMove) {
					if (!allowThresholdMove) {
						allowThresholdMove = true;
						s.touches.startX = s.touches.currentX;
						s.touches.startY = s.touches.currentY;
						currentTranslate = startTranslate;
						s.touches.diff = isH() ? s.touches.currentX - s.touches.startX : s.touches.currentY - s.touches.startY;
						return;
					}
				}
				else {
					currentTranslate = startTranslate;
					return;
				}
			}
			// Update active index in free mode
			if (s.params.freeMode || s.params.watchSlidesProgress) {
				s.updateActiveIndex();
			}
			if (s.params.freeMode) {
				//Velocity
				if (velocities.length === 0) {
					velocities.push({
						position: s.touches[isH() ? 'startX' : 'startY'],
						time: touchStartTime
					});
				}
				velocities.push({
					position: s.touches[isH() ? 'currentX' : 'currentY'],
					time: (new window.Date()).getTime()
				});
			}
			// Update progress
			s.updateProgress(currentTranslate);
			// Update translate
			s.setWrapperTranslate(currentTranslate);
		};
		s.onTouchEnd = function (e) {
			if (e.originalEvent) e = e.originalEvent;
			if (allowTouchCallbacks) {
				s.emit('onTouchEnd', s, e);
			}
			allowTouchCallbacks = false;
			if (!isTouched) return;
			//Return Grab Cursor
			if (s.params.grabCursor && isMoved && isTouched) {
				s.container[0].style.cursor = 'move';
				s.container[0].style.cursor = '-webkit-grab';
				s.container[0].style.cursor = '-moz-grab';
				s.container[0].style.cursor = 'grab';
			}
			
			// Time diff
			var touchEndTime = Date.now();
			var timeDiff = touchEndTime - touchStartTime;
			
			// Tap, doubleTap, Click
			if (s.allowClick) {
				s.updateClickedSlide(e);
				s.emit('onTap', s, e);
				if (timeDiff < 300 && (touchEndTime - lastClickTime) > 300) {
					if (clickTimeout) clearTimeout(clickTimeout);
					clickTimeout = setTimeout(function () {
						if (!s) return;
						if (s.params.paginationHide && s.paginationContainer.length > 0 && !$(e.target).hasClass(s.params.bulletClass)) {
							s.paginationContainer.toggleClass(s.params.paginationHiddenClass);
						}
						s.emit('onClick', s, e);
					}, 300);
					
				}
				if (timeDiff < 300 && (touchEndTime - lastClickTime) < 300) {
					if (clickTimeout) clearTimeout(clickTimeout);
					s.emit('onDoubleTap', s, e);
				}
			}
			
			lastClickTime = Date.now();
			setTimeout(function () {
				if (s) s.allowClick = true;
			}, 0);
			
			if (!isTouched || !isMoved || !s.swipeDirection || s.touches.diff === 0 || currentTranslate === startTranslate) {
				isTouched = isMoved = false;
				return;
			}
			isTouched = isMoved = false;
			
			var currentPos;
			if (s.params.followFinger) {
				currentPos = s.rtl ? s.translate : -s.translate;
			}
			else {
				currentPos = -currentTranslate;
			}
			if (s.params.freeMode) {
				if (currentPos < -s.minTranslate()) {
					s.slideTo(s.activeIndex);
					return;
				}
				else if (currentPos > -s.maxTranslate()) {
					if (s.slides.length < s.snapGrid.length) {
						s.slideTo(s.snapGrid.length - 1);
					}
					else {
						s.slideTo(s.slides.length - 1);
					}
					return;
				}
				
				if (s.params.freeModeMomentum) {
					if (velocities.length > 1) {
						var lastMoveEvent = velocities.pop(), velocityEvent = velocities.pop();
						
						var distance = lastMoveEvent.position - velocityEvent.position;
						var time = lastMoveEvent.time - velocityEvent.time;
						s.velocity = distance / time;
						s.velocity = s.velocity / 2;
						if (Math.abs(s.velocity) < s.params.freeModeMinimumVelocity) {
							s.velocity = 0;
						}
						// this implies that the user stopped moving a finger then released.
						// There would be no events with distance zero, so the last event is stale.
						if (time > 150 || (new window.Date().getTime() - lastMoveEvent.time) > 300) {
							s.velocity = 0;
						}
					} else {
						s.velocity = 0;
					}
					
					velocities.length = 0;
					var momentumDuration = 1000 * s.params.freeModeMomentumRatio;
					var momentumDistance = s.velocity * momentumDuration;
					
					var newPosition = s.translate + momentumDistance;
					if (s.rtl) newPosition = - newPosition;
					var doBounce = false;
					var afterBouncePosition;
					var bounceAmount = Math.abs(s.velocity) * 20 * s.params.freeModeMomentumBounceRatio;
					if (newPosition < s.maxTranslate()) {
						if (s.params.freeModeMomentumBounce) {
							if (newPosition + s.maxTranslate() < -bounceAmount) {
								newPosition = s.maxTranslate() - bounceAmount;
							}
							afterBouncePosition = s.maxTranslate();
							doBounce = true;
							allowMomentumBounce = true;
						}
						else {
							newPosition = s.maxTranslate();
						}
					}
					else if (newPosition > s.minTranslate()) {
						if (s.params.freeModeMomentumBounce) {
							if (newPosition - s.minTranslate() > bounceAmount) {
								newPosition = s.minTranslate() + bounceAmount;
							}
							afterBouncePosition = s.minTranslate();
							doBounce = true;
							allowMomentumBounce = true;
						}
						else {
							newPosition = s.minTranslate();
						}
					}
					else if (s.params.freeModeSticky) {
						var j = 0,
							nextSlide;
						for (j = 0; j < s.snapGrid.length; j += 1) {
							if (s.snapGrid[j] > -newPosition) {
								nextSlide = j;
								break;
							}
							
						}
						if (Math.abs(s.snapGrid[nextSlide] - newPosition) < Math.abs(s.snapGrid[nextSlide - 1] - newPosition) || s.swipeDirection === 'next') {
							newPosition = s.snapGrid[nextSlide];
						} else {
							newPosition = s.snapGrid[nextSlide - 1];
						}
						if (!s.rtl) newPosition = - newPosition;
					}
					//Fix duration
					if (s.velocity !== 0) {
						if (s.rtl) {
							momentumDuration = Math.abs((-newPosition - s.translate) / s.velocity);
						}
						else {
							momentumDuration = Math.abs((newPosition - s.translate) / s.velocity);
						}
					}
					else if (s.params.freeModeSticky) {
						s.slideReset();
						return;
					}
					
					if (s.params.freeModeMomentumBounce && doBounce) {
						s.updateProgress(afterBouncePosition);
						s.setWrapperTransition(momentumDuration);
						s.setWrapperTranslate(newPosition);
						s.onTransitionStart();
						s.animating = true;
						s.wrapper.transitionEnd(function () {
							if (!s || !allowMomentumBounce) return;
							s.emit('onMomentumBounce', s);
							
							s.setWrapperTransition(s.params.speed);
							s.setWrapperTranslate(afterBouncePosition);
							s.wrapper.transitionEnd(function () {
								if (!s) return;
								s.onTransitionEnd();
							});
						});
					} else if (s.velocity) {
						s.updateProgress(newPosition);
						s.setWrapperTransition(momentumDuration);
						s.setWrapperTranslate(newPosition);
						s.onTransitionStart();
						if (!s.animating) {
							s.animating = true;
							s.wrapper.transitionEnd(function () {
								if (!s) return;
								s.onTransitionEnd();
							});
						}
						
					} else {
						s.updateProgress(newPosition);
					}
					
					s.updateActiveIndex();
				}
				if (!s.params.freeModeMomentum || timeDiff >= s.params.longSwipesMs) {
					s.updateProgress();
					s.updateActiveIndex();
				}
				return;
			}
			
			// Find current slide
			var i, stopIndex = 0, groupSize = s.slidesSizesGrid[0];
			for (i = 0; i < s.slidesGrid.length; i += s.params.slidesPerGroup) {
				if (typeof s.slidesGrid[i + s.params.slidesPerGroup] !== 'undefined') {
					if (currentPos >= s.slidesGrid[i] && currentPos < s.slidesGrid[i + s.params.slidesPerGroup]) {
						stopIndex = i;
						groupSize = s.slidesGrid[i + s.params.slidesPerGroup] - s.slidesGrid[i];
					}
				}
				else {
					if (currentPos >= s.slidesGrid[i]) {
						stopIndex = i;
						groupSize = s.slidesGrid[s.slidesGrid.length - 1] - s.slidesGrid[s.slidesGrid.length - 2];
					}
				}
			}
			
			// Find current slide size
			var ratio = (currentPos - s.slidesGrid[stopIndex]) / groupSize;
			
			if (timeDiff > s.params.longSwipesMs) {
				// Long touches
				if (!s.params.longSwipes) {
					s.slideTo(s.activeIndex);
					return;
				}
				if (s.swipeDirection === 'next') {
					if (ratio >= s.params.longSwipesRatio) s.slideTo(stopIndex + s.params.slidesPerGroup);
					else s.slideTo(stopIndex);
					
				}
				if (s.swipeDirection === 'prev') {
					if (ratio > (1 - s.params.longSwipesRatio)) s.slideTo(stopIndex + s.params.slidesPerGroup);
					else s.slideTo(stopIndex);
				}
			}
			else {
				// Short swipes
				if (!s.params.shortSwipes) {
					s.slideTo(s.activeIndex);
					return;
				}
				if (s.swipeDirection === 'next') {
					s.slideTo(stopIndex + s.params.slidesPerGroup);
					
				}
				if (s.swipeDirection === 'prev') {
					s.slideTo(stopIndex);
				}
			}
		};
		/*=========================
		 Transitions
		 ===========================*/
		s._slideTo = function (slideIndex, speed) {
			return s.slideTo(slideIndex, speed, true, true);
		};
		s.slideTo = function (slideIndex, speed, runCallbacks, internal) {
			if (typeof runCallbacks === 'undefined') runCallbacks = true;
			if (typeof slideIndex === 'undefined') slideIndex = 0;
			if (slideIndex < 0) slideIndex = 0;
			s.snapIndex = Math.floor(slideIndex / s.params.slidesPerGroup);
			if (s.snapIndex >= s.snapGrid.length) s.snapIndex = s.snapGrid.length - 1;
			
			var translate = - s.snapGrid[s.snapIndex];
			
			// Stop autoplay
			if (s.params.autoplay && s.autoplaying) {
				if (internal || !s.params.autoplayDisableOnInteraction) {
					s.pauseAutoplay(speed);
				}
				else {
					s.stopAutoplay();
				}
			}
			// Update progress
			s.updateProgress(translate);
			
			// Normalize slideIndex
			for (var i = 0; i < s.slidesGrid.length; i++) {
				if (- Math.floor(translate * 100) >= Math.floor(s.slidesGrid[i] * 100)) {
					slideIndex = i;
				}
			}
			
			// Directions locks
			if (!s.params.allowSwipeToNext && translate < s.translate && translate < s.minTranslate()) {
				return false;
			}
			if (!s.params.allowSwipeToPrev && translate > s.translate && translate > s.maxTranslate()) {
				if ((s.activeIndex || 0) !== slideIndex ) return false;
			}
			
			// Update Index
			if (typeof speed === 'undefined') speed = s.params.speed;
			s.previousIndex = s.activeIndex || 0;
			s.activeIndex = slideIndex;
			
			// Update Height
			if (s.params.autoHeight) {
				s.updateAutoHeight();
			}
			
			if (translate === s.translate) {
				s.updateClasses();
				if (s.params.effect !== 'slide') {
					s.setWrapperTranslate(translate);
				}
				return false;
			}
			s.updateClasses();
			s.onTransitionStart(runCallbacks);
			
			if (speed === 0) {
				s.setWrapperTransition(0);
				s.setWrapperTranslate(translate);
				s.onTransitionEnd(runCallbacks);
			}
			else {
				s.setWrapperTransition(speed);
				s.setWrapperTranslate(translate);
				if (!s.animating) {
					s.animating = true;
					s.wrapper.transitionEnd(function () {
						if (!s) return;
						s.onTransitionEnd(runCallbacks);
					});
				}
				
			}
			
			return true;
		};
		
		s.onTransitionStart = function (runCallbacks) {
			if (typeof runCallbacks === 'undefined') runCallbacks = true;
			if (s.lazy) s.lazy.onTransitionStart();
			if (runCallbacks) {
				s.emit('onTransitionStart', s);
				if (s.activeIndex !== s.previousIndex) {
					s.emit('onSlideChangeStart', s);
					if (s.activeIndex > s.previousIndex) {
						s.emit('onSlideNextStart', s);
					}
					else {
						s.emit('onSlidePrevStart', s);
					}
				}
				
			}
		};
		s.onTransitionEnd = function (runCallbacks) {
			s.animating = false;
			s.setWrapperTransition(0);
			if (typeof runCallbacks === 'undefined') runCallbacks = true;
			if (s.lazy) s.lazy.onTransitionEnd();
			if (runCallbacks) {
				s.emit('onTransitionEnd', s);
				if (s.activeIndex !== s.previousIndex) {
					s.emit('onSlideChangeEnd', s);
					if (s.activeIndex > s.previousIndex) {
						s.emit('onSlideNextEnd', s);
					}
					else {
						s.emit('onSlidePrevEnd', s);
					}
				}
			}
			if (s.params.hashnav && s.hashnav) {
				s.hashnav.setHash();
			}
			
		};
		s.slideNext = function (runCallbacks, speed, internal) {
			if (s.params.loop) {
				if (s.animating) return false;
				s.fixLoop();
				var clientLeft = s.container[0].clientLeft;
				return s.slideTo(s.activeIndex + s.params.slidesPerGroup, speed, runCallbacks, internal);
			}
			else return s.slideTo(s.activeIndex + s.params.slidesPerGroup, speed, runCallbacks, internal);
		};
		s._slideNext = function (speed) {
			return s.slideNext(true, speed, true);
		};
		s.slidePrev = function (runCallbacks, speed, internal) {
			if (s.params.loop) {
				if (s.animating) return false;
				s.fixLoop();
				var clientLeft = s.container[0].clientLeft;
				return s.slideTo(s.activeIndex - 1, speed, runCallbacks, internal);
			}
			else return s.slideTo(s.activeIndex - 1, speed, runCallbacks, internal);
		};
		s._slidePrev = function (speed) {
			return s.slidePrev(true, speed, true);
		};
		s.slideReset = function (runCallbacks, speed, internal) {
			return s.slideTo(s.activeIndex, speed, runCallbacks);
		};
		
		/*=========================
		 Translate/transition helpers
		 ===========================*/
		s.setWrapperTransition = function (duration, byController) {
			s.wrapper.transition(duration);
			if (s.params.effect !== 'slide' && s.effects[s.params.effect]) {
				s.effects[s.params.effect].setTransition(duration);
			}
			if (s.params.parallax && s.parallax) {
				s.parallax.setTransition(duration);
			}
			if (s.params.scrollbar && s.scrollbar) {
				s.scrollbar.setTransition(duration);
			}
			if (s.params.control && s.controller) {
				s.controller.setTransition(duration, byController);
			}
			s.emit('onSetTransition', s, duration);
		};
		s.setWrapperTranslate = function (translate, updateActiveIndex, byController) {
			var x = 0, y = 0, z = 0;
			if (isH()) {
				x = s.rtl ? -translate : translate;
			}
			else {
				y = translate;
			}
			
			if (s.params.roundLengths) {
				x = round(x);
				y = round(y);
			}
			
			if (!s.params.virtualTranslate) {
				if (s.support.transforms3d) s.wrapper.transform('translate3d(' + x + 'px, ' + y + 'px, ' + z + 'px)');
				else s.wrapper.transform('translate(' + x + 'px, ' + y + 'px)');
			}
			
			s.translate = isH() ? x : y;
			
			// Check if we need to update progress
			var progress;
			var translatesDiff = s.maxTranslate() - s.minTranslate();
			if (translatesDiff === 0) {
				progress = 0;
			}
			else {
				progress = (translate - s.minTranslate()) / (translatesDiff);
			}
			if (progress !== s.progress) {
				s.updateProgress(translate);
			}
			
			if (updateActiveIndex) s.updateActiveIndex();
			if (s.params.effect !== 'slide' && s.effects[s.params.effect]) {
				s.effects[s.params.effect].setTranslate(s.translate);
			}
			if (s.params.parallax && s.parallax) {
				s.parallax.setTranslate(s.translate);
			}
			if (s.params.scrollbar && s.scrollbar) {
				s.scrollbar.setTranslate(s.translate);
			}
			if (s.params.control && s.controller) {
				s.controller.setTranslate(s.translate, byController);
			}
			s.emit('onSetTranslate', s, s.translate);
		};
		
		s.getTranslate = function (el, axis) {
			var matrix, curTransform, curStyle, transformMatrix;
			
			// automatic axis detection
			if (typeof axis === 'undefined') {
				axis = 'x';
			}
			
			if (s.params.virtualTranslate) {
				return s.rtl ? -s.translate : s.translate;
			}
			
			curStyle = window.getComputedStyle(el, null);
			if (window.WebKitCSSMatrix) {
				curTransform = curStyle.transform || curStyle.webkitTransform;
				if (curTransform.split(',').length > 6) {
					curTransform = curTransform.split(', ').map(function(a){
						return a.replace(',','.');
					}).join(', ');
				}
				// Some old versions of Webkit choke when 'none' is passed; pass
				// empty string instead in this case
				transformMatrix = new window.WebKitCSSMatrix(curTransform === 'none' ? '' : curTransform);
			}
			else {
				transformMatrix = curStyle.MozTransform || curStyle.OTransform || curStyle.MsTransform || curStyle.msTransform  || curStyle.transform || curStyle.getPropertyValue('transform').replace('translate(', 'matrix(1, 0, 0, 1,');
				matrix = transformMatrix.toString().split(',');
			}
			
			if (axis === 'x') {
				//Latest Chrome and webkits Fix
				if (window.WebKitCSSMatrix)
					curTransform = transformMatrix.m41;
				//Crazy IE10 Matrix
				else if (matrix.length === 16)
					curTransform = parseFloat(matrix[12]);
				//Normal Browsers
				else
					curTransform = parseFloat(matrix[4]);
			}
			if (axis === 'y') {
				//Latest Chrome and webkits Fix
				if (window.WebKitCSSMatrix)
					curTransform = transformMatrix.m42;
				//Crazy IE10 Matrix
				else if (matrix.length === 16)
					curTransform = parseFloat(matrix[13]);
				//Normal Browsers
				else
					curTransform = parseFloat(matrix[5]);
			}
			if (s.rtl && curTransform) curTransform = -curTransform;
			return curTransform || 0;
		};
		s.getWrapperTranslate = function (axis) {
			if (typeof axis === 'undefined') {
				axis = isH() ? 'x' : 'y';
			}
			return s.getTranslate(s.wrapper[0], axis);
		};
		
		/*=========================
		 Observer
		 ===========================*/
		s.observers = [];
		function initObserver(target, options) {
			options = options || {};
			// create an observer instance
			var ObserverFunc = window.MutationObserver || window.WebkitMutationObserver;
			var observer = new ObserverFunc(function (mutations) {
				mutations.forEach(function (mutation) {
					s.onResize(true);
					s.emit('onObserverUpdate', s, mutation);
				});
			});
			
			observer.observe(target, {
				attributes: typeof options.attributes === 'undefined' ? true : options.attributes,
				childList: typeof options.childList === 'undefined' ? true : options.childList,
				characterData: typeof options.characterData === 'undefined' ? true : options.characterData
			});
			
			s.observers.push(observer);
		}
		s.initObservers = function () {
			if (s.params.observeParents) {
				var containerParents = s.container.parents();
				for (var i = 0; i < containerParents.length; i++) {
					initObserver(containerParents[i]);
				}
			}
			
			// Observe container
			initObserver(s.container[0], {childList: false});
			
			// Observe wrapper
			initObserver(s.wrapper[0], {attributes: false});
		};
		s.disconnectObservers = function () {
			for (var i = 0; i < s.observers.length; i++) {
				s.observers[i].disconnect();
			}
			s.observers = [];
		};
		/*=========================
		 Loop
		 ===========================*/
		// Create looped slides
		s.createLoop = function () {
			// Remove duplicated slides
			s.wrapper.children('.' + s.params.slideClass + '.' + s.params.slideDuplicateClass).remove();
			
			var slides = s.wrapper.children('.' + s.params.slideClass);
			
			if(s.params.slidesPerView === 'auto' && !s.params.loopedSlides) s.params.loopedSlides = slides.length;
			
			s.loopedSlides = parseInt(s.params.loopedSlides || s.params.slidesPerView, 10);
			s.loopedSlides = s.loopedSlides + s.params.loopAdditionalSlides;
			if (s.loopedSlides > slides.length) {
				s.loopedSlides = slides.length;
			}
			
			var prependSlides = [], appendSlides = [], i;
			slides.each(function (index, el) {
				var slide = $(this);
				if (index < s.loopedSlides) appendSlides.push(el);
				if (index < slides.length && index >= slides.length - s.loopedSlides) prependSlides.push(el);
				slide.attr('data-swiper-slide-index', index);
			});
			for (i = 0; i < appendSlides.length; i++) {
				s.wrapper.append($(appendSlides[i].cloneNode(true)).addClass(s.params.slideDuplicateClass));
			}
			for (i = prependSlides.length - 1; i >= 0; i--) {
				s.wrapper.prepend($(prependSlides[i].cloneNode(true)).addClass(s.params.slideDuplicateClass));
			}
		};
		s.destroyLoop = function () {
			s.wrapper.children('.' + s.params.slideClass + '.' + s.params.slideDuplicateClass).remove();
			s.slides.removeAttr('data-swiper-slide-index');
		};
		s.fixLoop = function () {
			var newIndex;
			//Fix For Negative Oversliding
			if (s.activeIndex < s.loopedSlides) {
				newIndex = s.slides.length - s.loopedSlides * 3 + s.activeIndex;
				newIndex = newIndex + s.loopedSlides;
				s.slideTo(newIndex, 0, false, true);
			}
			//Fix For Positive Oversliding
			else if ((s.params.slidesPerView === 'auto' && s.activeIndex >= s.loopedSlides * 2) || (s.activeIndex > s.slides.length - s.params.slidesPerView * 2)) {
				newIndex = -s.slides.length + s.activeIndex + s.loopedSlides;
				newIndex = newIndex + s.loopedSlides;
				s.slideTo(newIndex, 0, false, true);
			}
		};
		/*=========================
		 Append/Prepend/Remove Slides
		 ===========================*/
		s.appendSlide = function (slides) {
			if (s.params.loop) {
				s.destroyLoop();
			}
			if (typeof slides === 'object' && slides.length) {
				for (var i = 0; i < slides.length; i++) {
					if (slides[i]) s.wrapper.append(slides[i]);
				}
			}
			else {
				s.wrapper.append(slides);
			}
			if (s.params.loop) {
				s.createLoop();
			}
			if (!(s.params.observer && s.support.observer)) {
				s.update(true);
			}
		};
		s.prependSlide = function (slides) {
			if (s.params.loop) {
				s.destroyLoop();
			}
			var newActiveIndex = s.activeIndex + 1;
			if (typeof slides === 'object' && slides.length) {
				for (var i = 0; i < slides.length; i++) {
					if (slides[i]) s.wrapper.prepend(slides[i]);
				}
				newActiveIndex = s.activeIndex + slides.length;
			}
			else {
				s.wrapper.prepend(slides);
			}
			if (s.params.loop) {
				s.createLoop();
			}
			if (!(s.params.observer && s.support.observer)) {
				s.update(true);
			}
			s.slideTo(newActiveIndex, 0, false);
		};
		s.removeSlide = function (slidesIndexes) {
			if (s.params.loop) {
				s.destroyLoop();
				s.slides = s.wrapper.children('.' + s.params.slideClass);
			}
			var newActiveIndex = s.activeIndex,
				indexToRemove;
			if (typeof slidesIndexes === 'object' && slidesIndexes.length) {
				for (var i = 0; i < slidesIndexes.length; i++) {
					indexToRemove = slidesIndexes[i];
					if (s.slides[indexToRemove]) s.slides.eq(indexToRemove).remove();
					if (indexToRemove < newActiveIndex) newActiveIndex--;
				}
				newActiveIndex = Math.max(newActiveIndex, 0);
			}
			else {
				indexToRemove = slidesIndexes;
				if (s.slides[indexToRemove]) s.slides.eq(indexToRemove).remove();
				if (indexToRemove < newActiveIndex) newActiveIndex--;
				newActiveIndex = Math.max(newActiveIndex, 0);
			}
			
			if (s.params.loop) {
				s.createLoop();
			}
			
			if (!(s.params.observer && s.support.observer)) {
				s.update(true);
			}
			if (s.params.loop) {
				s.slideTo(newActiveIndex + s.loopedSlides, 0, false);
			}
			else {
				s.slideTo(newActiveIndex, 0, false);
			}
			
		};
		s.removeAllSlides = function () {
			var slidesIndexes = [];
			for (var i = 0; i < s.slides.length; i++) {
				slidesIndexes.push(i);
			}
			s.removeSlide(slidesIndexes);
		};
		
		
		/*=========================
		 Effects
		 ===========================*/
		s.effects = {
			fade: {
				setTranslate: function () {
					for (var i = 0; i < s.slides.length; i++) {
						var slide = s.slides.eq(i);
						var offset = slide[0].swiperSlideOffset;
						var tx = -offset;
						if (!s.params.virtualTranslate) tx = tx - s.translate;
						var ty = 0;
						if (!isH()) {
							ty = tx;
							tx = 0;
						}
						var slideOpacity = s.params.fade.crossFade ?
							Math.max(1 - Math.abs(slide[0].progress), 0) :
						1 + Math.min(Math.max(slide[0].progress, -1), 0);
						slide
							.css({
								opacity: slideOpacity
							})
							.transform('translate3d(' + tx + 'px, ' + ty + 'px, 0px)');
						
					}
					
				},
				setTransition: function (duration) {
					s.slides.transition(duration);
					if (s.params.virtualTranslate && duration !== 0) {
						var eventTriggered = false;
						s.slides.transitionEnd(function () {
							if (eventTriggered) return;
							if (!s) return;
							eventTriggered = true;
							s.animating = false;
							var triggerEvents = ['webkitTransitionEnd', 'transitionend', 'oTransitionEnd', 'MSTransitionEnd', 'msTransitionEnd'];
							for (var i = 0; i < triggerEvents.length; i++) {
								s.wrapper.trigger(triggerEvents[i]);
							}
						});
					}
				}
			},
			cube: {
				setTranslate: function () {
					var wrapperRotate = 0, cubeShadow;
					if (s.params.cube.shadow) {
						if (isH()) {
							cubeShadow = s.wrapper.find('.swiper-cube-shadow');
							if (cubeShadow.length === 0) {
								cubeShadow = $('<div class="swiper-cube-shadow"></div>');
								s.wrapper.append(cubeShadow);
							}
							cubeShadow.css({height: s.width + 'px'});
						}
						else {
							cubeShadow = s.container.find('.swiper-cube-shadow');
							if (cubeShadow.length === 0) {
								cubeShadow = $('<div class="swiper-cube-shadow"></div>');
								s.container.append(cubeShadow);
							}
						}
					}
					for (var i = 0; i < s.slides.length; i++) {
						var slide = s.slides.eq(i);
						var slideAngle = i * 90;
						var round = Math.floor(slideAngle / 360);
						if (s.rtl) {
							slideAngle = -slideAngle;
							round = Math.floor(-slideAngle / 360);
						}
						var progress = Math.max(Math.min(slide[0].progress, 1), -1);
						var tx = 0, ty = 0, tz = 0;
						if (i % 4 === 0) {
							tx = - round * 4 * s.size;
							tz = 0;
						}
						else if ((i - 1) % 4 === 0) {
							tx = 0;
							tz = - round * 4 * s.size;
						}
						else if ((i - 2) % 4 === 0) {
							tx = s.size + round * 4 * s.size;
							tz = s.size;
						}
						else if ((i - 3) % 4 === 0) {
							tx = - s.size;
							tz = 3 * s.size + s.size * 4 * round;
						}
						if (s.rtl) {
							tx = -tx;
						}
						
						if (!isH()) {
							ty = tx;
							tx = 0;
						}
						
						var transform = 'rotateX(' + (isH() ? 0 : -slideAngle) + 'deg) rotateY(' + (isH() ? slideAngle : 0) + 'deg) translate3d(' + tx + 'px, ' + ty + 'px, ' + tz + 'px)';
						if (progress <= 1 && progress > -1) {
							wrapperRotate = i * 90 + progress * 90;
							if (s.rtl) wrapperRotate = -i * 90 - progress * 90;
						}
						slide.transform(transform);
						if (s.params.cube.slideShadows) {
							//Set shadows
							var shadowBefore = isH() ? slide.find('.swiper-slide-shadow-left') : slide.find('.swiper-slide-shadow-top');
							var shadowAfter = isH() ? slide.find('.swiper-slide-shadow-right') : slide.find('.swiper-slide-shadow-bottom');
							if (shadowBefore.length === 0) {
								shadowBefore = $('<div class="swiper-slide-shadow-' + (isH() ? 'left' : 'top') + '"></div>');
								slide.append(shadowBefore);
							}
							if (shadowAfter.length === 0) {
								shadowAfter = $('<div class="swiper-slide-shadow-' + (isH() ? 'right' : 'bottom') + '"></div>');
								slide.append(shadowAfter);
							}
							var shadowOpacity = slide[0].progress;
							if (shadowBefore.length) shadowBefore[0].style.opacity = -slide[0].progress;
							if (shadowAfter.length) shadowAfter[0].style.opacity = slide[0].progress;
						}
					}
					s.wrapper.css({
						'-webkit-transform-origin': '50% 50% -' + (s.size / 2) + 'px',
						'-moz-transform-origin': '50% 50% -' + (s.size / 2) + 'px',
						'-ms-transform-origin': '50% 50% -' + (s.size / 2) + 'px',
						'transform-origin': '50% 50% -' + (s.size / 2) + 'px'
					});
					
					if (s.params.cube.shadow) {
						if (isH()) {
							cubeShadow.transform('translate3d(0px, ' + (s.width / 2 + s.params.cube.shadowOffset) + 'px, ' + (-s.width / 2) + 'px) rotateX(90deg) rotateZ(0deg) scale(' + (s.params.cube.shadowScale) + ')');
						}
						else {
							var shadowAngle = Math.abs(wrapperRotate) - Math.floor(Math.abs(wrapperRotate) / 90) * 90;
							var multiplier = 1.5 - (Math.sin(shadowAngle * 2 * Math.PI / 360) / 2 + Math.cos(shadowAngle * 2 * Math.PI / 360) / 2);
							var scale1 = s.params.cube.shadowScale,
								scale2 = s.params.cube.shadowScale / multiplier,
								offset = s.params.cube.shadowOffset;
							cubeShadow.transform('scale3d(' + scale1 + ', 1, ' + scale2 + ') translate3d(0px, ' + (s.height / 2 + offset) + 'px, ' + (-s.height / 2 / scale2) + 'px) rotateX(-90deg)');
						}
					}
					var zFactor = (s.isSafari || s.isUiWebView) ? (-s.size / 2) : 0;
					s.wrapper.transform('translate3d(0px,0,' + zFactor + 'px) rotateX(' + (isH() ? 0 : wrapperRotate) + 'deg) rotateY(' + (isH() ? -wrapperRotate : 0) + 'deg)');
				},
				setTransition: function (duration) {
					s.slides.transition(duration).find('.swiper-slide-shadow-top, .swiper-slide-shadow-right, .swiper-slide-shadow-bottom, .swiper-slide-shadow-left').transition(duration);
					if (s.params.cube.shadow && !isH()) {
						s.container.find('.swiper-cube-shadow').transition(duration);
					}
				}
			},
			coverflow: {
				setTranslate: function () {
					var transform = s.translate;
					var center = isH() ? -transform + s.width / 2 : -transform + s.height / 2;
					var rotate = isH() ? s.params.coverflow.rotate: -s.params.coverflow.rotate;
					var translate = s.params.coverflow.depth;
					//Each slide offset from center
					for (var i = 0, length = s.slides.length; i < length; i++) {
						var slide = s.slides.eq(i);
						var slideSize = s.slidesSizesGrid[i];
						var slideOffset = slide[0].swiperSlideOffset;
						var offsetMultiplier = (center - slideOffset - slideSize / 2) / slideSize * s.params.coverflow.modifier;
						
						var rotateY = isH() ? rotate * offsetMultiplier : 0;
						var rotateX = isH() ? 0 : rotate * offsetMultiplier;
						// var rotateZ = 0
						var translateZ = -translate * Math.abs(offsetMultiplier);
						
						var translateY = isH() ? 0 : s.params.coverflow.stretch * (offsetMultiplier);
						var translateX = isH() ? s.params.coverflow.stretch * (offsetMultiplier) : 0;
						
						//Fix for ultra small values
						if (Math.abs(translateX) < 0.001) translateX = 0;
						if (Math.abs(translateY) < 0.001) translateY = 0;
						if (Math.abs(translateZ) < 0.001) translateZ = 0;
						if (Math.abs(rotateY) < 0.001) rotateY = 0;
						if (Math.abs(rotateX) < 0.001) rotateX = 0;
						
						var slideTransform = 'translate3d(' + translateX + 'px,' + translateY + 'px,' + translateZ + 'px)  rotateX(' + rotateX + 'deg) rotateY(' + rotateY + 'deg)';
						
						slide.transform(slideTransform);
						slide[0].style.zIndex = -Math.abs(Math.round(offsetMultiplier)) + 1;
						if (s.params.coverflow.slideShadows) {
							//Set shadows
							var shadowBefore = isH() ? slide.find('.swiper-slide-shadow-left') : slide.find('.swiper-slide-shadow-top');
							var shadowAfter = isH() ? slide.find('.swiper-slide-shadow-right') : slide.find('.swiper-slide-shadow-bottom');
							if (shadowBefore.length === 0) {
								shadowBefore = $('<div class="swiper-slide-shadow-' + (isH() ? 'left' : 'top') + '"></div>');
								slide.append(shadowBefore);
							}
							if (shadowAfter.length === 0) {
								shadowAfter = $('<div class="swiper-slide-shadow-' + (isH() ? 'right' : 'bottom') + '"></div>');
								slide.append(shadowAfter);
							}
							if (shadowBefore.length) shadowBefore[0].style.opacity = offsetMultiplier > 0 ? offsetMultiplier : 0;
							if (shadowAfter.length) shadowAfter[0].style.opacity = (-offsetMultiplier) > 0 ? -offsetMultiplier : 0;
						}
					}
					
					//Set correct perspective for IE10
					if (s.browser.ie) {
						var ws = s.wrapper[0].style;
						ws.perspectiveOrigin = center + 'px 50%';
					}
				},
				setTransition: function (duration) {
					s.slides.transition(duration).find('.swiper-slide-shadow-top, .swiper-slide-shadow-right, .swiper-slide-shadow-bottom, .swiper-slide-shadow-left').transition(duration);
				}
			}
		};
		
		/*=========================
		 Images Lazy Loading
		 ===========================*/
		s.lazy = {
			initialImageLoaded: false,
			loadImageInSlide: function (index, loadInDuplicate) {
				if (typeof index === 'undefined') return;
				if (typeof loadInDuplicate === 'undefined') loadInDuplicate = true;
				if (s.slides.length === 0) return;
				
				var slide = s.slides.eq(index);
				var img = slide.find('.swiper-lazy:not(.swiper-lazy-loaded):not(.swiper-lazy-loading)');
				if (slide.hasClass('swiper-lazy') && !slide.hasClass('swiper-lazy-loaded') && !slide.hasClass('swiper-lazy-loading')) {
					img = img.add(slide[0]);
				}
				if (img.length === 0) return;
				
				img.each(function () {
					var _img = $(this);
					_img.addClass('swiper-lazy-loading');
					var background = _img.attr('data-background');
					var src = _img.attr('data-src'),
						srcset = _img.attr('data-srcset');
					s.loadImage(_img[0], (src || background), srcset, false, function () {
						if (background) {
							_img.css('background-image', 'url(' + background + ')');
							_img.removeAttr('data-background');
						}
						else {
							if (srcset) {
								_img.attr('srcset', srcset);
								_img.removeAttr('data-srcset');
							}
							if (src) {
								_img.attr('src', src);
								_img.removeAttr('data-src');
							}
							
						}
						
						_img.addClass('swiper-lazy-loaded').removeClass('swiper-lazy-loading');
						slide.find('.swiper-lazy-preloader, .preloader').remove();
						if (s.params.loop && loadInDuplicate) {
							var slideOriginalIndex = slide.attr('data-swiper-slide-index');
							if (slide.hasClass(s.params.slideDuplicateClass)) {
								var originalSlide = s.wrapper.children('[data-swiper-slide-index="' + slideOriginalIndex + '"]:not(.' + s.params.slideDuplicateClass + ')');
								s.lazy.loadImageInSlide(originalSlide.index(), false);
							}
							else {
								var duplicatedSlide = s.wrapper.children('.' + s.params.slideDuplicateClass + '[data-swiper-slide-index="' + slideOriginalIndex + '"]');
								s.lazy.loadImageInSlide(duplicatedSlide.index(), false);
							}
						}
						s.emit('onLazyImageReady', s, slide[0], _img[0]);
					});
					
					s.emit('onLazyImageLoad', s, slide[0], _img[0]);
				});
				
			},
			load: function () {
				var i;
				if (s.params.watchSlidesVisibility) {
					s.wrapper.children('.' + s.params.slideVisibleClass).each(function () {
						s.lazy.loadImageInSlide($(this).index());
					});
				}
				else {
					if (s.params.slidesPerView > 1) {
						for (i = s.activeIndex; i < s.activeIndex + s.params.slidesPerView ; i++) {
							if (s.slides[i]) s.lazy.loadImageInSlide(i);
						}
					}
					else {
						s.lazy.loadImageInSlide(s.activeIndex);
					}
				}
				if (s.params.lazyLoadingInPrevNext) {
					if (s.params.slidesPerView > 1) {
						// Next Slides
						for (i = s.activeIndex + s.params.slidesPerView; i < s.activeIndex + s.params.slidesPerView + s.params.slidesPerView; i++) {
							if (s.slides[i]) s.lazy.loadImageInSlide(i);
						}
						// Prev Slides
						for (i = s.activeIndex - s.params.slidesPerView; i < s.activeIndex ; i++) {
							if (s.slides[i]) s.lazy.loadImageInSlide(i);
						}
					}
					else {
						var nextSlide = s.wrapper.children('.' + s.params.slideNextClass);
						if (nextSlide.length > 0) s.lazy.loadImageInSlide(nextSlide.index());
						
						var prevSlide = s.wrapper.children('.' + s.params.slidePrevClass);
						if (prevSlide.length > 0) s.lazy.loadImageInSlide(prevSlide.index());
					}
				}
			},
			onTransitionStart: function () {
				if (s.params.lazyLoading) {
					if (s.params.lazyLoadingOnTransitionStart || (!s.params.lazyLoadingOnTransitionStart && !s.lazy.initialImageLoaded)) {
						s.lazy.load();
					}
				}
			},
			onTransitionEnd: function () {
				if (s.params.lazyLoading && !s.params.lazyLoadingOnTransitionStart) {
					s.lazy.load();
				}
			}
		};
		
		
		/*=========================
		 Scrollbar
		 ===========================*/
		s.scrollbar = {
			isTouched: false,
			setDragPosition: function (e) {
				var sb = s.scrollbar;
				var x = 0, y = 0;
				var translate;
				var pointerPosition = isH() ?
					((e.type === 'touchstart' || e.type === 'touchmove') ? e.targetTouches[0].pageX : e.pageX || e.clientX) :
					((e.type === 'touchstart' || e.type === 'touchmove') ? e.targetTouches[0].pageY : e.pageY || e.clientY) ;
				var position = (pointerPosition) - sb.track.offset()[isH() ? 'left' : 'top'] - sb.dragSize / 2;
				var positionMin = -s.minTranslate() * sb.moveDivider;
				var positionMax = -s.maxTranslate() * sb.moveDivider;
				if (position < positionMin) {
					position = positionMin;
				}
				else if (position > positionMax) {
					position = positionMax;
				}
				position = -position / sb.moveDivider;
				s.updateProgress(position);
				s.setWrapperTranslate(position, true);
			},
			dragStart: function (e) {
				var sb = s.scrollbar;
				sb.isTouched = true;
				e.preventDefault();
				e.stopPropagation();
				
				sb.setDragPosition(e);
				clearTimeout(sb.dragTimeout);
				
				sb.track.transition(0);
				if (s.params.scrollbarHide) {
					sb.track.css('opacity', 1);
				}
				s.wrapper.transition(100);
				sb.drag.transition(100);
				s.emit('onScrollbarDragStart', s);
			},
			dragMove: function (e) {
				var sb = s.scrollbar;
				if (!sb.isTouched) return;
				if (e.preventDefault) e.preventDefault();
				else e.returnValue = false;
				sb.setDragPosition(e);
				s.wrapper.transition(0);
				sb.track.transition(0);
				sb.drag.transition(0);
				s.emit('onScrollbarDragMove', s);
			},
			dragEnd: function (e) {
				var sb = s.scrollbar;
				if (!sb.isTouched) return;
				sb.isTouched = false;
				if (s.params.scrollbarHide) {
					clearTimeout(sb.dragTimeout);
					sb.dragTimeout = setTimeout(function () {
						sb.track.css('opacity', 0);
						sb.track.transition(400);
					}, 1000);
					
				}
				s.emit('onScrollbarDragEnd', s);
				if (s.params.scrollbarSnapOnRelease) {
					s.slideReset();
				}
			},
			enableDraggable: function () {
				var sb = s.scrollbar;
				var target = s.support.touch ? sb.track : document;
				$(sb.track).on(s.touchEvents.start, sb.dragStart);
				$(target).on(s.touchEvents.move, sb.dragMove);
				$(target).on(s.touchEvents.end, sb.dragEnd);
			},
			disableDraggable: function () {
				var sb = s.scrollbar;
				var target = s.support.touch ? sb.track : document;
				$(sb.track).off(s.touchEvents.start, sb.dragStart);
				$(target).off(s.touchEvents.move, sb.dragMove);
				$(target).off(s.touchEvents.end, sb.dragEnd);
			},
			set: function () {
				if (!s.params.scrollbar) return;
				var sb = s.scrollbar;
				sb.track = $(s.params.scrollbar);
				sb.drag = sb.track.find('.swiper-scrollbar-drag');
				if (sb.drag.length === 0) {
					sb.drag = $('<div class="swiper-scrollbar-drag"></div>');
					sb.track.append(sb.drag);
				}
				sb.drag[0].style.width = '';
				sb.drag[0].style.height = '';
				sb.trackSize = isH() ? sb.track[0].offsetWidth : sb.track[0].offsetHeight;
				
				sb.divider = s.size / s.virtualSize;
				sb.moveDivider = sb.divider * (sb.trackSize / s.size);
				sb.dragSize = sb.trackSize * sb.divider;
				
				if (isH()) {
					sb.drag[0].style.width = sb.dragSize + 'px';
				}
				else {
					sb.drag[0].style.height = sb.dragSize + 'px';
				}
				
				if (sb.divider >= 1) {
					sb.track[0].style.display = 'none';
				}
				else {
					sb.track[0].style.display = '';
				}
				if (s.params.scrollbarHide) {
					sb.track[0].style.opacity = 0;
				}
			},
			setTranslate: function () {
				if (!s.params.scrollbar) return;
				var diff;
				var sb = s.scrollbar;
				var translate = s.translate || 0;
				var newPos;
				
				var newSize = sb.dragSize;
				newPos = (sb.trackSize - sb.dragSize) * s.progress;
				if (s.rtl && isH()) {
					newPos = -newPos;
					if (newPos > 0) {
						newSize = sb.dragSize - newPos;
						newPos = 0;
					}
					else if (-newPos + sb.dragSize > sb.trackSize) {
						newSize = sb.trackSize + newPos;
					}
				}
				else {
					if (newPos < 0) {
						newSize = sb.dragSize + newPos;
						newPos = 0;
					}
					else if (newPos + sb.dragSize > sb.trackSize) {
						newSize = sb.trackSize - newPos;
					}
				}
				if (isH()) {
					if (s.support.transforms3d) {
						sb.drag.transform('translate3d(' + (newPos) + 'px, 0, 0)');
					}
					else {
						sb.drag.transform('translateX(' + (newPos) + 'px)');
					}
					sb.drag[0].style.width = newSize + 'px';
				}
				else {
					if (s.support.transforms3d) {
						sb.drag.transform('translate3d(0px, ' + (newPos) + 'px, 0)');
					}
					else {
						sb.drag.transform('translateY(' + (newPos) + 'px)');
					}
					sb.drag[0].style.height = newSize + 'px';
				}
				if (s.params.scrollbarHide) {
					clearTimeout(sb.timeout);
					sb.track[0].style.opacity = 1;
					sb.timeout = setTimeout(function () {
						sb.track[0].style.opacity = 0;
						sb.track.transition(400);
					}, 1000);
				}
			},
			setTransition: function (duration) {
				if (!s.params.scrollbar) return;
				s.scrollbar.drag.transition(duration);
			}
		};
		
		/*=========================
		 Controller
		 ===========================*/
		s.controller = {
			LinearSpline: function (x, y) {
				this.x = x;
				this.y = y;
				this.lastIndex = x.length - 1;
				// Given an x value (x2), return the expected y2 value:
				// (x1,y1) is the known point before given value,
				// (x3,y3) is the known point after given value.
				var i1, i3;
				var l = this.x.length;
				
				this.interpolate = function (x2) {
					if (!x2) return 0;
					
					// Get the indexes of x1 and x3 (the array indexes before and after given x2):
					i3 = binarySearch(this.x, x2);
					i1 = i3 - 1;
					
					// We have our indexes i1 & i3, so we can calculate already:
					// y2 := ((x2−x1) × (y3−y1)) ÷ (x3−x1) + y1
					return ((x2 - this.x[i1]) * (this.y[i3] - this.y[i1])) / (this.x[i3] - this.x[i1]) + this.y[i1];
				};
				
				var binarySearch = (function() {
					var maxIndex, minIndex, guess;
					return function(array, val) {
						minIndex = -1;
						maxIndex = array.length;
						while (maxIndex - minIndex > 1)
							if (array[guess = maxIndex + minIndex >> 1] <= val) {
								minIndex = guess;
							} else {
								maxIndex = guess;
							}
						return maxIndex;
					};
				})();
			},
			//xxx: for now i will just save one spline function to to
			getInterpolateFunction: function(c){
				if(!s.controller.spline) s.controller.spline = s.params.loop ?
					new s.controller.LinearSpline(s.slidesGrid, c.slidesGrid) :
					new s.controller.LinearSpline(s.snapGrid, c.snapGrid);
			},
			setTranslate: function (translate, byController) {
				var controlled = s.params.control;
				var multiplier, controlledTranslate;
				function setControlledTranslate(c) {
					// this will create an Interpolate function based on the snapGrids
					// x is the Grid of the scrolled scroller and y will be the controlled scroller
					// it makes sense to create this only once and recall it for the interpolation
					// the function does a lot of value caching for performance
					translate = c.rtl && c.params.direction === 'horizontal' ? -s.translate : s.translate;
					if (s.params.controlBy === 'slide') {
						s.controller.getInterpolateFunction(c);
						// i am not sure why the values have to be multiplicated this way, tried to invert the snapGrid
						// but it did not work out
						controlledTranslate = -s.controller.spline.interpolate(-translate);
					}
					
					if(!controlledTranslate || s.params.controlBy === 'container'){
						multiplier = (c.maxTranslate() - c.minTranslate()) / (s.maxTranslate() - s.minTranslate());
						controlledTranslate = (translate - s.minTranslate()) * multiplier + c.minTranslate();
					}
					
					if (s.params.controlInverse) {
						controlledTranslate = c.maxTranslate() - controlledTranslate;
					}
					c.updateProgress(controlledTranslate);
					c.setWrapperTranslate(controlledTranslate, false, s);
					c.updateActiveIndex();
				}
				if (s.isArray(controlled)) {
					for (var i = 0; i < controlled.length; i++) {
						if (controlled[i] !== byController && controlled[i] instanceof Swiper) {
							setControlledTranslate(controlled[i]);
						}
					}
				}
				else if (controlled instanceof Swiper && byController !== controlled) {
					
					setControlledTranslate(controlled);
				}
			},
			setTransition: function (duration, byController) {
				var controlled = s.params.control;
				var i;
				function setControlledTransition(c) {
					c.setWrapperTransition(duration, s);
					if (duration !== 0) {
						c.onTransitionStart();
						c.wrapper.transitionEnd(function(){
							if (!controlled) return;
							if (c.params.loop && s.params.controlBy === 'slide') {
								c.fixLoop();
							}
							c.onTransitionEnd();
							
						});
					}
				}
				if (s.isArray(controlled)) {
					for (i = 0; i < controlled.length; i++) {
						if (controlled[i] !== byController && controlled[i] instanceof Swiper) {
							setControlledTransition(controlled[i]);
						}
					}
				}
				else if (controlled instanceof Swiper && byController !== controlled) {
					setControlledTransition(controlled);
				}
			}
		};
		
		/*=========================
		 Hash Navigation
		 ===========================*/
		s.hashnav = {
			init: function () {
				if (!s.params.hashnav) return;
				s.hashnav.initialized = true;
				var hash = document.location.hash.replace('#', '');
				if (!hash) return;
				var speed = 0;
				for (var i = 0, length = s.slides.length; i < length; i++) {
					var slide = s.slides.eq(i);
					var slideHash = slide.attr('data-hash');
					if (slideHash === hash && !slide.hasClass(s.params.slideDuplicateClass)) {
						var index = slide.index();
						s.slideTo(index, speed, s.params.runCallbacksOnInit, true);
					}
				}
			},
			setHash: function () {
				if (!s.hashnav.initialized || !s.params.hashnav) return;
				document.location.hash = s.slides.eq(s.activeIndex).attr('data-hash') || '';
			}
		};
		
		/*=========================
		 Keyboard Control
		 ===========================*/
		function handleKeyboard(e) {
			if (e.originalEvent) e = e.originalEvent; //jquery fix
			var kc = e.keyCode || e.charCode;
			// Directions locks
			if (!s.params.allowSwipeToNext && (isH() && kc === 39 || !isH() && kc === 40)) {
				return false;
			}
			if (!s.params.allowSwipeToPrev && (isH() && kc === 37 || !isH() && kc === 38)) {
				return false;
			}
			if (e.shiftKey || e.altKey || e.ctrlKey || e.metaKey) {
				return;
			}
			if (document.activeElement && document.activeElement.nodeName && (document.activeElement.nodeName.toLowerCase() === 'input' || document.activeElement.nodeName.toLowerCase() === 'textarea')) {
				return;
			}

			/* Amethyst content */
			if ($('#obsidian-overlay.oo-active').length) {
				// Skip if obsidian is active
				return;
			}
			/* /Amethyst content */

			if (kc === 37 || kc === 39 || kc === 38 || kc === 40) {
				var inView = false;
				//Check that swiper should be inside of visible area of window
				if (s.container.parents('.swiper-slide').length > 0 && s.container.parents('.swiper-slide-active').length === 0) {
					return;
				}
				var windowScroll = {
					left: window.pageXOffset,
					top: window.pageYOffset
				};
				var windowWidth = window.innerWidth;
				var windowHeight = window.innerHeight;
				var swiperOffset = s.container.offset();
				if (s.rtl) swiperOffset.left = swiperOffset.left - s.container[0].scrollLeft;
				var swiperCoord = [
					[swiperOffset.left, swiperOffset.top],
					[swiperOffset.left + s.width, swiperOffset.top],
					[swiperOffset.left, swiperOffset.top + s.height],
					[swiperOffset.left + s.width, swiperOffset.top + s.height]
				];
				for (var i = 0; i < swiperCoord.length; i++) {
					var point = swiperCoord[i];
					if (
						point[0] >= windowScroll.left && point[0] <= windowScroll.left + windowWidth &&
						point[1] >= windowScroll.top && point[1] <= windowScroll.top + windowHeight
					) {
						inView = true;
					}
					
				}
				if (!inView) return;
			}
			if (isH()) {
				if (kc === 37 || kc === 39) {
					if (e.preventDefault) e.preventDefault();
					else e.returnValue = false;
				}
				if ((kc === 39 && !s.rtl) || (kc === 37 && s.rtl)) s.slideNext();
				if ((kc === 37 && !s.rtl) || (kc === 39 && s.rtl)) s.slidePrev();
			}
			else {
				if (kc === 38 || kc === 40) {
					if (e.preventDefault) e.preventDefault();
					else e.returnValue = false;
				}
				if (kc === 40) s.slideNext();
				if (kc === 38) s.slidePrev();
			}
		}
		s.disableKeyboardControl = function () {
			$(document).off('keydown', handleKeyboard);
		};
		s.enableKeyboardControl = function () {
			$(document).on('keydown', handleKeyboard);
		};
		
		
		/*=========================
		 Mousewheel Control
		 ===========================*/
		s.mousewheel = {
			event: false,
			lastScrollTime: (new window.Date()).getTime()
		};
		if (s.params.mousewheelControl) {
			try {
				new window.WheelEvent('wheel');
				s.mousewheel.event = 'wheel';
			} catch (e) {}
			
			if (!s.mousewheel.event && document.onmousewheel !== undefined) {
				s.mousewheel.event = 'mousewheel';
			}
			if (!s.mousewheel.event) {
				s.mousewheel.event = 'DOMMouseScroll';
			}
		}
		function handleMousewheel(e) {
			if (e.originalEvent) e = e.originalEvent; //jquery fix
			var we = s.mousewheel.event;
			var delta = 0;
			//Opera & IE
			if (e.detail) delta = -e.detail;
			//WebKits
			else if (we === 'mousewheel') {
				if (s.params.mousewheelForceToAxis) {
					if (isH()) {
						if (Math.abs(e.wheelDeltaX) > Math.abs(e.wheelDeltaY)) delta = e.wheelDeltaX;
						else return;
					}
					else {
						if (Math.abs(e.wheelDeltaY) > Math.abs(e.wheelDeltaX)) delta = e.wheelDeltaY;
						else return;
					}
				}
				else {
					delta = e.wheelDelta;
				}
			}
			//Old FireFox
			else if (we === 'DOMMouseScroll') delta = -e.detail;
			//New FireFox
			else if (we === 'wheel') {
				if (s.params.mousewheelForceToAxis) {
					if (isH()) {
						if (Math.abs(e.deltaX) > Math.abs(e.deltaY)) delta = -e.deltaX;
						else return;
					}
					else {
						if (Math.abs(e.deltaY) > Math.abs(e.deltaX)) delta = -e.deltaY;
						else return;
					}
				}
				else {
					delta = Math.abs(e.deltaX) > Math.abs(e.deltaY) ? - e.deltaX : - e.deltaY;
				}
			}
			if (delta === 0) return;
			
			if (s.params.mousewheelInvert) delta = -delta;
			
			if (!s.params.freeMode) {
				if ((new window.Date()).getTime() - s.mousewheel.lastScrollTime > 60) {
					if (delta < 0) {
						if ((!s.isEnd || s.params.loop) && !s.animating) s.slideNext();
						else if (s.params.mousewheelReleaseOnEdges) return true;
					}
					else {
						if ((!s.isBeginning || s.params.loop) && !s.animating) s.slidePrev();
						else if (s.params.mousewheelReleaseOnEdges) return true;
					}
				}
				s.mousewheel.lastScrollTime = (new window.Date()).getTime();
				
			}
			else {
				//Freemode or scrollContainer:
				var position = s.getWrapperTranslate() + delta * s.params.mousewheelSensitivity;
				var wasBeginning = s.isBeginning,
					wasEnd = s.isEnd;
				
				if (position >= s.minTranslate()) position = s.minTranslate();
				if (position <= s.maxTranslate()) position = s.maxTranslate();
				
				s.setWrapperTransition(0);
				s.setWrapperTranslate(position);
				s.updateProgress();
				s.updateActiveIndex();
				
				if (!wasBeginning && s.isBeginning || !wasEnd && s.isEnd) {
					s.updateClasses();
				}
				
				if (s.params.freeModeSticky) {
					clearTimeout(s.mousewheel.timeout);
					s.mousewheel.timeout = setTimeout(function () {
						s.slideReset();
					}, 300);
				}
				
				// Return page scroll on edge positions
				if (position === 0 || position === s.maxTranslate()) return;
			}
			if (s.params.autoplay) s.stopAutoplay();
			
			if (e.preventDefault) e.preventDefault();
			else e.returnValue = false;
			return false;
		}
		s.disableMousewheelControl = function () {
			if (!s.mousewheel.event) return false;
			s.container.off(s.mousewheel.event, handleMousewheel);
			return true;
		};
		
		s.enableMousewheelControl = function () {
			if (!s.mousewheel.event) return false;
			s.container.on(s.mousewheel.event, handleMousewheel);
			return true;
		};
		
		/*=========================
		 Parallax
		 ===========================*/
		function setParallaxTransform(el, progress) {
			el = $(el);
			var p, pX, pY;
			
			p = el.attr('data-swiper-parallax') || '0';
			pX = el.attr('data-swiper-parallax-x');
			pY = el.attr('data-swiper-parallax-y');
			if (pX || pY) {
				pX = pX || '0';
				pY = pY || '0';
			}
			else {
				if (isH()) {
					pX = p;
					pY = '0';
				}
				else {
					pY = p;
					pX = '0';
				}
			}
			if ((pX).indexOf('%') >= 0) {
				pX = parseInt(pX, 10) * progress + '%';
			}
			else {
				pX = pX * progress + 'px' ;
			}
			if ((pY).indexOf('%') >= 0) {
				pY = parseInt(pY, 10) * progress + '%';
			}
			else {
				pY = pY * progress + 'px' ;
			}
			el.transform('translate3d(' + pX + ', ' + pY + ',0px)');
		}
		s.parallax = {
			setTranslate: function () {
				s.container.children('[data-swiper-parallax], [data-swiper-parallax-x], [data-swiper-parallax-y]').each(function(){
					setParallaxTransform(this, s.progress);
					
				});
				s.slides.each(function () {
					var slide = $(this);
					slide.find('[data-swiper-parallax], [data-swiper-parallax-x], [data-swiper-parallax-y]').each(function () {
						var progress = Math.min(Math.max(slide[0].progress, -1), 1);
						setParallaxTransform(this, progress);
					});
				});
			},
			setTransition: function (duration) {
				if (typeof duration === 'undefined') duration = s.params.speed;
				s.container.find('[data-swiper-parallax], [data-swiper-parallax-x], [data-swiper-parallax-y]').each(function(){
					var el = $(this);
					var parallaxDuration = parseInt(el.attr('data-swiper-parallax-duration'), 10) || duration;
					if (duration === 0) parallaxDuration = 0;
					el.transition(parallaxDuration);
				});
			}
		};
		
		
		/*=========================
		 Plugins API. Collect all and init all plugins
		 ===========================*/
		s._plugins = [];
		for (var plugin in s.plugins) {
			var p = s.plugins[plugin](s, s.params[plugin]);
			if (p) s._plugins.push(p);
		}
		// Method to call all plugins event/method
		s.callPlugins = function (eventName) {
			for (var i = 0; i < s._plugins.length; i++) {
				if (eventName in s._plugins[i]) {
					s._plugins[i][eventName](arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
				}
			}
		};
		
		/*=========================
		 Events/Callbacks/Plugins Emitter
		 ===========================*/
		function normalizeEventName (eventName) {
			if (eventName.indexOf('on') !== 0) {
				if (eventName[0] !== eventName[0].toUpperCase()) {
					eventName = 'on' + eventName[0].toUpperCase() + eventName.substring(1);
				}
				else {
					eventName = 'on' + eventName;
				}
			}
			return eventName;
		}
		s.emitterEventListeners = {
			
		};
		s.emit = function (eventName) {
			// Trigger callbacks
			if (s.params[eventName]) {
				s.params[eventName](arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
			}
			var i;
			// Trigger events
			if (s.emitterEventListeners[eventName]) {
				for (i = 0; i < s.emitterEventListeners[eventName].length; i++) {
					s.emitterEventListeners[eventName][i](arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
				}
			}
			// Trigger plugins
			if (s.callPlugins) s.callPlugins(eventName, arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
		};
		s.on = function (eventName, handler) {
			eventName = normalizeEventName(eventName);
			if (!s.emitterEventListeners[eventName]) s.emitterEventListeners[eventName] = [];
			s.emitterEventListeners[eventName].push(handler);
			return s;
		};
		s.off = function (eventName, handler) {
			var i;
			eventName = normalizeEventName(eventName);
			if (typeof handler === 'undefined') {
				// Remove all handlers for such event
				s.emitterEventListeners[eventName] = [];
				return s;
			}
			if (!s.emitterEventListeners[eventName] || s.emitterEventListeners[eventName].length === 0) return;
			for (i = 0; i < s.emitterEventListeners[eventName].length; i++) {
				if(s.emitterEventListeners[eventName][i] === handler) s.emitterEventListeners[eventName].splice(i, 1);
			}
			return s;
		};
		s.once = function (eventName, handler) {
			eventName = normalizeEventName(eventName);
			var _handler = function () {
				handler(arguments[0], arguments[1], arguments[2], arguments[3], arguments[4]);
				s.off(eventName, _handler);
			};
			s.on(eventName, _handler);
			return s;
		};
		
		// Accessibility tools
		s.a11y = {
			makeFocusable: function ($el) {
				$el.attr('tabIndex', '0');
				return $el;
			},
			addRole: function ($el, role) {
				$el.attr('role', role);
				return $el;
			},
			
			addLabel: function ($el, label) {
				$el.attr('aria-label', label);
				return $el;
			},
			
			disable: function ($el) {
				$el.attr('aria-disabled', true);
				return $el;
			},
			
			enable: function ($el) {
				$el.attr('aria-disabled', false);
				return $el;
			},
			
			onEnterKey: function (event) {
				if (event.keyCode !== 13) return;
				if ($(event.target).is(s.params.nextButton)) {
					s.onClickNext(event);
					if (s.isEnd) {
						s.a11y.notify(s.params.lastSlideMessage);
					}
					else {
						s.a11y.notify(s.params.nextSlideMessage);
					}
				}
				else if ($(event.target).is(s.params.prevButton)) {
					s.onClickPrev(event);
					if (s.isBeginning) {
						s.a11y.notify(s.params.firstSlideMessage);
					}
					else {
						s.a11y.notify(s.params.prevSlideMessage);
					}
				}
				if ($(event.target).is('.' + s.params.bulletClass)) {
					$(event.target)[0].click();
				}
			},
			
			liveRegion: $('<span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>'),
			
			notify: function (message) {
				var notification = s.a11y.liveRegion;
				if (notification.length === 0) return;
				notification.html('');
				notification.html(message);
			},
			init: function () {
				// Setup accessibility
				if (s.params.nextButton) {
					var nextButton = $(s.params.nextButton);
					s.a11y.makeFocusable(nextButton);
					s.a11y.addRole(nextButton, 'button');
					s.a11y.addLabel(nextButton, s.params.nextSlideMessage);
				}
				if (s.params.prevButton) {
					var prevButton = $(s.params.prevButton);
					s.a11y.makeFocusable(prevButton);
					s.a11y.addRole(prevButton, 'button');
					s.a11y.addLabel(prevButton, s.params.prevSlideMessage);
				}
				
				$(s.container).append(s.a11y.liveRegion);
			},
			initPagination: function () {
				if (s.params.pagination && s.params.paginationClickable && s.bullets && s.bullets.length) {
					s.bullets.each(function () {
						var bullet = $(this);
						s.a11y.makeFocusable(bullet);
						s.a11y.addRole(bullet, 'button');
						s.a11y.addLabel(bullet, s.params.paginationBulletMessage.replace(/{{index}}/, bullet.index() + 1));
					});
				}
			},
			destroy: function () {
				if (s.a11y.liveRegion && s.a11y.liveRegion.length > 0) s.a11y.liveRegion.remove();
			}
		};
		
		
		/*=========================
		 Init/Destroy
		 ===========================*/
		s.init = function () {
			if (s.params.loop) s.createLoop();
			s.updateContainerSize();
			s.updateSlidesSize();
			s.updatePagination();
			if (s.params.scrollbar && s.scrollbar) {
				s.scrollbar.set();
				if (s.params.scrollbarDraggable) {
					s.scrollbar.enableDraggable();
				}
			}
			if (s.params.effect !== 'slide' && s.effects[s.params.effect]) {
				if (!s.params.loop) s.updateProgress();
				s.effects[s.params.effect].setTranslate();
			}
			if (s.params.loop) {
				s.slideTo(s.params.initialSlide + s.loopedSlides, 0, s.params.runCallbacksOnInit);
			}
			else {
				s.slideTo(s.params.initialSlide, 0, s.params.runCallbacksOnInit);
				if (s.params.initialSlide === 0) {
					if (s.parallax && s.params.parallax) s.parallax.setTranslate();
					if (s.lazy && s.params.lazyLoading) {
						s.lazy.load();
						s.lazy.initialImageLoaded = true;
					}
				}
			}
			s.attachEvents();
			if (s.params.observer && s.support.observer) {
				s.initObservers();
			}
			if (s.params.preloadImages && !s.params.lazyLoading) {
				s.preloadImages();
			}
			if (s.params.autoplay) {
				s.startAutoplay();
			}
			if (s.params.keyboardControl) {
				if (s.enableKeyboardControl) s.enableKeyboardControl();
			}
			if (s.params.mousewheelControl) {
				if (s.enableMousewheelControl) s.enableMousewheelControl();
			}
			if (s.params.hashnav) {
				if (s.hashnav) s.hashnav.init();
			}
			if (s.params.a11y && s.a11y) s.a11y.init();
			s.emit('onInit', s);
		};
		
		// Cleanup dynamic styles
		s.cleanupStyles = function () {
			// Container
			s.container.removeClass(s.classNames.join(' ')).removeAttr('style');
			
			// Wrapper
			s.wrapper.removeAttr('style');
			
			// Slides
			if (s.slides && s.slides.length) {
				s.slides
					.removeClass([
						s.params.slideVisibleClass,
						s.params.slideActiveClass,
						s.params.slideNextClass,
						s.params.slidePrevClass
					].join(' '))
					.removeAttr('style')
					.removeAttr('data-swiper-column')
					.removeAttr('data-swiper-row');
			}
			
			// Pagination/Bullets
			if (s.paginationContainer && s.paginationContainer.length) {
				s.paginationContainer.removeClass(s.params.paginationHiddenClass);
			}
			if (s.bullets && s.bullets.length) {
				s.bullets.removeClass(s.params.bulletActiveClass);
			}
			
			// Buttons
			if (s.params.prevButton) $(s.params.prevButton).removeClass(s.params.buttonDisabledClass);
			if (s.params.nextButton) $(s.params.nextButton).removeClass(s.params.buttonDisabledClass);
			
			// Scrollbar
			if (s.params.scrollbar && s.scrollbar) {
				if (s.scrollbar.track && s.scrollbar.track.length) s.scrollbar.track.removeAttr('style');
				if (s.scrollbar.drag && s.scrollbar.drag.length) s.scrollbar.drag.removeAttr('style');
			}
		};
		
		// Destroy
		s.destroy = function (deleteInstance, cleanupStyles) {
			// Detach evebts
			s.detachEvents();
			// Stop autoplay
			s.stopAutoplay();
			// Disable draggable
			if (s.params.scrollbar && s.scrollbar) {
				if (s.params.scrollbarDraggable) {
					s.scrollbar.disableDraggable();
				}
			}
			// Destroy loop
			if (s.params.loop) {
				s.destroyLoop();
			}
			// Cleanup styles
			if (cleanupStyles) {
				s.cleanupStyles();
			}
			// Disconnect observer
			s.disconnectObservers();
			// Disable keyboard/mousewheel
			if (s.params.keyboardControl) {
				if (s.disableKeyboardControl) s.disableKeyboardControl();
			}
			if (s.params.mousewheelControl) {
				if (s.disableMousewheelControl) s.disableMousewheelControl();
			}
			// Disable a11y
			if (s.params.a11y && s.a11y) s.a11y.destroy();
			// Destroy callback
			s.emit('onDestroy');
			// Delete instance
			if (deleteInstance !== false) s = null;
		};
		
		s.init();
		
		
		
		// Return swiper instance
		return s;
	};
	
	
	/*==================================================
	 Prototype
	 ====================================================*/
	Swiper.prototype = {
		isSafari: (function () {
			var ua = navigator.userAgent.toLowerCase();
			return (ua.indexOf('safari') >= 0 && ua.indexOf('chrome') < 0 && ua.indexOf('android') < 0);
		})(),
		isUiWebView: /(iPhone|iPod|iPad).*AppleWebKit(?!.*Safari)/i.test(navigator.userAgent),
		isArray: function (arr) {
			return Object.prototype.toString.apply(arr) === '[object Array]';
		},
		/*==================================================
		 Browser
		 ====================================================*/
		browser: {
			ie: window.navigator.pointerEnabled || window.navigator.msPointerEnabled,
			ieTouch: (window.navigator.msPointerEnabled && window.navigator.msMaxTouchPoints > 1) || (window.navigator.pointerEnabled && window.navigator.maxTouchPoints > 1)
		},
		/*==================================================
		 Devices
		 ====================================================*/
		device: (function () {
			var ua = navigator.userAgent;
			var android = ua.match(/(Android);?[\s\/]+([\d.]+)?/);
			var ipad = ua.match(/(iPad).*OS\s([\d_]+)/);
			var ipod = ua.match(/(iPod)(.*OS\s([\d_]+))?/);
			var iphone = !ipad && ua.match(/(iPhone\sOS)\s([\d_]+)/);
			return {
				ios: ipad || iphone || ipod,
				android: android
			};
		})(),
		/*==================================================
		 Feature Detection
		 ====================================================*/
		support: {
			touch : (window.Modernizr && Modernizr.touch === true) || (function () {
				return !!(('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch);
			})(),
			
			transforms3d : (window.Modernizr && Modernizr.csstransforms3d === true) || (function () {
				var div = document.createElement('div').style;
				return ('webkitPerspective' in div || 'MozPerspective' in div || 'OPerspective' in div || 'MsPerspective' in div || 'perspective' in div);
			})(),
			
			flexbox: (function () {
				var div = document.createElement('div').style;
				var styles = ('alignItems webkitAlignItems webkitBoxAlign msFlexAlign mozBoxAlign webkitFlexDirection msFlexDirection mozBoxDirection mozBoxOrient webkitBoxDirection webkitBoxOrient').split(' ');
				for (var i = 0; i < styles.length; i++) {
					if (styles[i] in div) return true;
				}
			})(),
			
			observer: (function () {
				return ('MutationObserver' in window || 'WebkitMutationObserver' in window);
			})()
		},
		/*==================================================
		 Plugins
		 ====================================================*/
		plugins: {}
	};
	
	
	/*===========================
	 Get Dom libraries
	 ===========================*/
	var swiperDomPlugins = ['jQuery', 'Zepto', 'Dom7'];
	for (var i = 0; i < swiperDomPlugins.length; i++) {
		if (window[swiperDomPlugins[i]]) {
			addLibraryPlugin(window[swiperDomPlugins[i]]);
		}
	}
	// Required DOM Plugins
	var domLib;
	if (typeof Dom7 === 'undefined') {
		domLib = window.Dom7 || window.Zepto || window.jQuery;
	}
	else {
		domLib = Dom7;
	}
	
	/*===========================
	 Add .swiper plugin from Dom libraries
	 ===========================*/
	function addLibraryPlugin(lib) {
		lib.fn.swiper = function (params) {
			var firstInstance;
			lib(this).each(function () {
				var s = new Swiper(this, params);
				if (!firstInstance) firstInstance = s;
			});
			return firstInstance;
		};
	}
	
	if (domLib) {
		if (!('transitionEnd' in domLib.fn)) {
			domLib.fn.transitionEnd = function (callback) {
				var events = ['webkitTransitionEnd', 'transitionend', 'oTransitionEnd', 'MSTransitionEnd', 'msTransitionEnd'],
					i, j, dom = this;
				function fireCallBack(e) {
					/*jshint validthis:true */
					if (e.target !== this) return;
					callback.call(this, e);
					for (i = 0; i < events.length; i++) {
						dom.off(events[i], fireCallBack);
					}
				}
				if (callback) {
					for (i = 0; i < events.length; i++) {
						dom.on(events[i], fireCallBack);
					}
				}
				return this;
			};
		}
		if (!('transform' in domLib.fn)) {
			domLib.fn.transform = function (transform) {
				for (var i = 0; i < this.length; i++) {
					var elStyle = this[i].style;
					elStyle.webkitTransform = elStyle.MsTransform = elStyle.msTransform = elStyle.MozTransform = elStyle.OTransform = elStyle.transform = transform;
				}
				return this;
			};
		}
		if (!('transition' in domLib.fn)) {
			domLib.fn.transition = function (duration) {
				if (typeof duration !== 'string') {
					duration = duration + 'ms';
				}
				for (var i = 0; i < this.length; i++) {
					var elStyle = this[i].style;
					elStyle.webkitTransitionDuration = elStyle.MsTransitionDuration = elStyle.msTransitionDuration = elStyle.MozTransitionDuration = elStyle.OTransitionDuration = elStyle.transitionDuration = duration;
				}
				return this;
			};
		}
	}
	
	window.Swiper = Swiper;
})();
/*===========================
 Swiper AMD Export
 ===========================*/
if (typeof(module) !== 'undefined')
{
	module.exports = window.Swiper;
}
else if (typeof define === 'function' && define.amd) {
	define([], function () {
		'use strict';
		return window.Swiper;
	});
}
// require "shards/obsidian/_obsidian.js"
/**
 * jquery-simple-datetimepicker (jquery.simple-dtpicker.js)
 * v1.13.0
 * (c) Masanori Ohgita.
 * https://github.com/mugifly/jquery-simple-datetimepicker
 **/

(function($) {
	var lang = {
		en: {
			days: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
			months: [ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" ],
			sep: '-',
			format: 'YYYY-MM-DD hh:mm',
			prevMonth: 'Previous month',
			nextMonth: 'Next month',
			today: 'Today'
		},
		ru: {
			days: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
			sep: '.',
			months: [ "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" ],
			format: 'DD.MM.YYYY hh:mm'
		}
		//
	};
	/* ----- */

	/**
	 PickerHandler Object
	 **/
	var PickerHandler = function($picker, $input){
		this.$pickerObject = $picker;
		this.$inputObject = $input;
	};

	/* Get a picker */
	PickerHandler.prototype.getPicker = function(){
		return this.$pickerObject;
	};

	/* Get a input-field */
	PickerHandler.prototype.getInput = function(){
		return this.$inputObject;
	};

	/* Get the display state of a picker */
	PickerHandler.prototype.isShow = function(){
		var is_show = true;
		if (this.$pickerObject.css('display') == 'none') {
			is_show = false;
		}
		return is_show;
	};

	/* Show a picker */
	PickerHandler.prototype.show = function(){
		var $picker = this.$pickerObject;
		var $input = this.$inputObject;

		$picker.show();


		// Amethyst
		// Get options
		var opt = $picker.data('opt');
		// Add inverted/normal class
		if ($input.hasClass(opt.invertedClass) || (opt.invertedParents && $input.closest('.' + opt.invertedClass).length)) {
			// Element have 'inverted' class, or have parents with this class
			$picker.removeClass('datepicker--normal').addClass('datepicker--inverted');
		} else {
			// Normal class
			$picker.removeClass('datepicker--inverted').addClass('datepicker--normal');
		}
		var this2 = this;
		setTimeout(function() {
			// Clear cache
			$picker.addClass('datepicker_active');
			this2._relocate();
		}, 10);
		// eof Amethyst

		ActivePickerId = $input.data('pickerId');

		// Moved relocate into setTimeout
		//this._relocate();
	};

	/* Hide a picker */
	PickerHandler.prototype.hide = function(){
		var $picker = this.$pickerObject;
		var $input = this.$inputObject;
		setTimeout(function() {
			$picker.hide();
		}, 300);

		$picker.removeClass('datepicker_active');

	};

	/* Get a selected date from a picker */
	PickerHandler.prototype.getDate = function(){
		var $picker = this.$pickerObject;
		var $input = this.$inputObject;
		return getPickedDate($picker);
	};

	/* Set a specific date to a picker */
	PickerHandler.prototype.setDate = function(date){
		var $picker = this.$pickerObject;
		var $input = this.$inputObject;
		if (!isObj('Date', date)) {
			date = new Date(date);
		}

		draw_date($picker, {
			"isAnim": true,
			"isOutputToInputObject": true
		}, date);
	};

	/* Set a specific min date to a picker and redraw */
	PickerHandler.prototype.setMinDate = function(date){
		var $picker = this.$pickerObject;
		var $input = this.$inputObject;
		if (!isObj('Date', date)) {
			date = new Date(date);
		}
		$picker.data("minDate", date);
		if ($input.val()) {
			datepicked = new Date(getPickedDate($picker));
			draw_date($picker, {
				"isAnim": true,
				"isOutputToInputObject": true
			}, ((datepicked > date) ? datepicked : date));
		} else {
			draw_date($picker, {
				"isAnim": true,
				"isOutputToInputObject": false
			}, date);
		}
	};

	/* Set a specific max date to a picker and redraw */
	PickerHandler.prototype.setMaxDate = function(date){
		var $picker = this.$pickerObject;
		var $input = this.$inputObject;
		if (!isObj('Date', date)) {
			date = new Date(date);
		}
		$picker.data("maxDate", date);
		if ($input.val()) {
			datepicked = new Date(getPickedDate($picker));
			draw_date($picker, {
				"isAnim": true,
				"isOutputToInputObject": true
			}, ((datepicked < date) ? datepicked : date));
		} else {
			draw_date($picker, {
				"isAnim": true,
				"isOutputToInputObject": false
			}, date);
		}
	};

	/* Destroy a picker */
	PickerHandler.prototype.destroy = function(){
		var $picker = this.$pickerObject;
		var picker_id = $picker.data('pickerId');
		PickerObjects[picker_id] = null;
		$picker.remove();
	};

	/* Relocate a picker to position of the appended input-field. */
	PickerHandler.prototype._relocate = function(){
		var $picker = this.$pickerObject;
		var $input = this.$inputObject;

		if ($picker.hasClass('datepicker_active') && $input != null && $picker.data('isInline') === false) { // Float mode
			// Move position of a picker - vertical
			var input_outer_height = $input.outerHeight({'margin': true});
			if (!isObj('Number', input_outer_height)) {
				input_outer_height = $input.outerHeight();
			}
			var picker_outer_height = $picker.outerHeight({'margin': true});
			if (!isObj('Number', picker_outer_height)) {
				picker_outer_height = $picker.outerHeight();
			}

			// Set width to assure date and time are side by side
			/*if($(".datepicker_calendar", $picker).width() !== 0 && $(".datepicker_timelist", $picker).width() !== 0){
				$picker.parent().width($(".datepicker_calendar", $picker).width() + $(".datepicker_timelist", $picker).width() + 6);
			}*/
			if(parseInt($(window).height()) <= ($input.offset().top - $(document).scrollTop() + input_outer_height + picker_outer_height) ){
				// Display to top of an input-field
				$picker.parent().css('top', ($input.offset().top - picker_outer_height) + 'px');
				// Amethyst //
				$picker.addClass('dropdown-top').removeClass('dropdown-bottom');
				// eof Amethyst //
			} else {
				// Display to bottom of an input-field
				$picker.parent().css('top', ($input.offset().top + input_outer_height) + 'px');
				// Amethyst //
				$picker.removeClass('dropdown-top').addClass('dropdown-bottom');
				// eof Amethyst //
			}
			/*
			// Move position of a picker - horizontal
			if($picker.parent().width() + $input.offset().left > $(window).width()) {
				// Display left side stick to window
				$picker.parent().css('left', (($(window).width() - $picker.parent().width()) / 2) + 'px');
			} else {
				// Display left side stick to input
				$picker.parent().css('left', $input.offset().left + 'px');
			}
			*/
			/* Amethyst content */
			var pickWidth = $picker.parent().outerWidth();
			var inpLeft = $input.offset().left;
			var inpWidth = $input.outerWidth();
			var WW = $(window).width();
			if (pickWidth < 100 || (pickWidth + inpLeft > WW && inpLeft + inpWidth - pickWidth < 0)) {
				// Time only or not enough space for side align, align center
				$picker.parent().css('left', (inpLeft + (inpWidth / 2) - (pickWidth / 2)) + 'px');
				$picker.addClass('datepicker--align-center');
				$picker.removeClass('datepicker--align-left datepicker--align-right');
			} else if(pickWidth + inpLeft < WW) {
				// Display left side stick to input
				$picker.parent().css('left', (inpLeft) + 'px');
				$picker.addClass('datepicker--align-left');
				$picker.removeClass('datepicker--align-center datepicker--align-right');
			} else {
				// Display right side stick to input
				$picker.parent().css('left', (inpLeft + inpWidth - pickWidth) + 'px');
				$picker.addClass('datepicker--align-right');
				$picker.removeClass('datepicker--align-center datepicker--align-left');
			}
			// Display on most top of the z-index
			$picker.parent().css('z-index', 100000);
		}
	};

	/* ----- */

	var PickerObjects = [];
	var InputObjects = [];
	var ActivePickerId = -1;

	var getParentPickerObject = function(obj) {
		return $(obj).closest('.datepicker');
	};

	var getPickersInputObject = function($obj) {
		var $picker = getParentPickerObject($obj);
		if ($picker.data("inputObjectId") != null) {
			return $(InputObjects[$picker.data("inputObjectId")]);
		}
		return null;
	};

	var setToNow = function($obj) {
		var $picker = getParentPickerObject($obj);
		var date = new Date();
		draw($picker, {
			"isAnim": true,
			"isOutputToInputObject": true
		}, date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes());
	};

	var beforeMonth = function($obj) {

		var $picker = getParentPickerObject($obj);

		if ($picker.data('stateAllowBeforeMonth') === false) { // Not allowed
			return;
		}

		var date = getShownDate($picker);
		var targetMonth_lastDay = new Date(date.getFullYear(), date.getMonth(), 0).getDate();
		if (targetMonth_lastDay < date.getDate()) {
			date.setDate(targetMonth_lastDay);
		}
		draw($picker, {
			"isAnim": true,
			"isOutputToInputObject": false,
			"keepPickedDate": true
		}, date.getFullYear(), date.getMonth() - 1, date.getDate(), date.getHours(), date.getMinutes());

		var todayDate = new Date();
		var isCurrentYear = todayDate.getFullYear() == date.getFullYear();
		var isCurrentMonth = isCurrentYear && todayDate.getMonth() == date.getMonth();

		if (!isCurrentMonth || !$picker.data("futureOnly")) {
			if (targetMonth_lastDay < date.getDate()) {
				date.setDate(targetMonth_lastDay);
			}
			var newdate = new Date(date.getFullYear(), date.getMonth() - 1, date.getDate(), date.getHours(), date.getMinutes());
			if ($picker.data("minDate") && newdate < $picker.data("minDate"))
				newdate = $picker.data("minDate");
			draw($picker, {
				"isAnim": true,
				"isOutputToInputObject": false,
				"keepPickedDate": true
			}, newdate.getFullYear(), newdate.getMonth(), newdate.getDate(), newdate.getHours(), newdate.getMinutes());
		}

		// Manual relocate
		var $input = getPickersInputObject($obj);
		var handler = new PickerHandler($picker, $input);
		handler._relocate();
	};

	var nextMonth = function($obj) {
		var $picker = getParentPickerObject($obj);
		var date = getShownDate($picker);
		var targetMonth_lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
		if (targetMonth_lastDay < date.getDate()) {
			date.setDate(targetMonth_lastDay);
		}
		draw($picker, {
			"isAnim": true,
			"isOutputToInputObject": false,
			"keepPickedDate": true
		}, date.getFullYear(), date.getMonth() + 1, date.getDate(), date.getHours(), date.getMinutes());

		// Check a last date of a next month
		if (getLastDate(date.getFullYear(), date.getMonth() + 1) < date.getDate()) {
			date.setDate(getLastDate(date.getFullYear(), date.getMonth() + 1));
		}
		var newdate = new Date(date.getFullYear(), date.getMonth() + 1, date.getDate(), date.getHours(), date.getMinutes());
		if ($picker.data("maxDate") && newdate > $picker.data("maxDate"))
			newdate = $picker.data("maxDate");
		draw($picker, {
			"isAnim": true,
			"isOutputToInputObject": false,
			"keepPickedDate": true
		}, newdate.getFullYear(), newdate.getMonth(), newdate.getDate(), newdate.getHours(), newdate.getMinutes());

		// Manual relocate
		var $input = getPickersInputObject($obj);
		var handler = new PickerHandler($picker, $input);
		handler._relocate();
	};

	/**
	 Check a last date of a specified year and month
	 **/
	var getLastDate = function(year, month) {
		var date = new Date(year, month + 1, 0);
		return date.getDate();
	};

	var getDateFormat = function (format, locale, is_date_only, is_time_only) {
		if (format == "default") {
			// Default format
			format = translate(locale,'format');
			if (is_date_only) {
				// Convert the format to date-only (ex: YYYY/MM/DD)
				format = format.substring(0, format.search(' '));
			}
			else if (is_time_only) {
				format = format.substring(format.search(' ') + 1);
			}
		}
		return format; // Return date-format
	};

	var normalizeYear = function (year) {
		if (year < 99) { // change year for 4 digits
			var date = new Date();
			return parseInt(year) + parseInt(date.getFullYear().toString().substr(0, 2) + "00");
		}
		return year;
	};
	var parseDate = function (str, opt_date_format) {

		var re, m, date;
		if(opt_date_format != null){
			// Parse date & time with date-format

			// Match a string with date format
			var df = opt_date_format.replace(/(-|\/)/g, '[-\/]')
				.replace(/YYYY/gi, '(\\d{2,4})')
				.replace(/(YY|MM|DD|HH|hh|mm)/g, '(\\d{1,2})')
				.replace(/(M|D|H|h|m)/g, '(\\d{1,2})')
				.replace(/(tt|TT)/g, '([aApP][mM])');
			re = new RegExp(df);
			m = re.exec(str);
			if( m != null){

				// Generate the formats array (convert-table)
				var formats = [];
				var format_buf = '';
				var format_before_c = '';
				var df_ = opt_date_format;
				while (df_ != null && 0 < df_.length) {
					var format_c = df_.substring(0, 1); df_ = df_.substring(1, df_.length);
					if (format_before_c != format_c) {
						if(/(YYYY|YY|MM|DD|mm|dd|M|D|HH|H|hh|h|m|tt|TT)/.test(format_buf)){
							formats.push( format_buf );
							format_buf = '';
						} else {
							format_buf = '';
						}
					}
					format_buf += format_c;
					format_before_c = format_c;
				}
				if (format_buf !== '' && /(YYYY|YY|MM|DD|mm|dd|M|D|HH|H|hh|h|m|tt|TT)/.test(format_buf)){
					formats.push( format_buf );
				}

				// Convert a string (with convert-table) to a date object
				var year, month, day, hour, min;
				var is_successful = false;
				var pm = false;
				var H = false;
				for(var i = 0; i < formats.length; i++){
					if(m.length < i){
						break;
					}

					var f = formats[i];
					var d = m[i+1]; // Matched part of date
					if(f == 'YYYY'){
						year = normalizeYear(d);
						is_successful = true;
					} else if(f == 'YY'){
						year = parseInt(d) + 2000;
						is_successful = true;
					} else if(f == 'MM' || f == 'M'){
						month = parseInt(d) - 1;
						is_successful = true;
					} else if(f == 'DD' || f == 'D'){
						day = d;
						is_successful = true;
					} else if(f == 'hh' || f == 'h'){
						hour = d;
						is_successful = true;
					} else if(f == 'HH' || f == 'H'){
						hour = d;
						H = true;
						is_successful = true;
					} else if(f == 'mm' || f == 'm'){
						min = d;
						is_successful = true;
					} else if(f == 'tt' || f == 'TT'){
						if(d == 'pm' || d == 'PM'){
							pm = true;
						}
						is_successful = true;
					}
				}
				if(H) {
					if(pm) {
						if(hour != 12) {
							hour = parseInt(hour) + 12;
						}
					} else if(hour == 12) {
						hour = 0;
					}
				}
				if (is_successful == true) {
					if (!year) {
						year = 2000;
					}
					if (!month) {
						month = 1;
					}
					if (!day) {
						day = 1;
					}
					if (!hour) {
						hour = 0;
					}
					if (!min) {
						min = 1;
					}
				}
				date = new Date(year, month, day, hour, min);

				if(is_successful === true && isNaN(date) === false && isNaN(date.getDate()) === false){ // Parse successful
					return date;
				}
			}
		}

		// Parse date & time with common format
		re = /^(\d{2,4})[-\/](\d{1,2})[-\/](\d{1,2}) (\d{1,2}):(\d{1,2})$/;
		m = re.exec(str);
		if (m !== null) {
			m[1] = normalizeYear(m[1]);
			date = new Date(m[1], m[2] - 1, m[3], m[4], m[5]);
		} else {
			// Parse for date-only
			re = /^(\d{2,4})[-\/](\d{1,2})[-\/](\d{1,2})$/;
			m = re.exec(str);
			if(m !== null) {
				m[1] = normalizeYear(m[1]);
				date = new Date(m[1], m[2] - 1, m[3]);
			}
		}

		if(isNaN(date) === false && isNaN(date.getDate()) === false){ // Parse successful
			return date;
		}
		return false;
	};
	var getFormattedDate = function(date, date_format) {
		if(date == null){
			date = new Date();
		}

		var y = date.getFullYear();
		var m = date.getMonth() + 1;
		var d = date.getDate();
		var hou = date.getHours();
		var min = date.getMinutes();

		date_format = date_format.replace(/YYYY/gi, y)
			.replace(/YY/g, y - 2000)/* century */
			.replace(/MM/g, zpadding(m))
			.replace(/M/g, m)
			.replace(/DD/g, zpadding(d))
			.replace(/D/g, d)
			.replace(/hh/g, zpadding(hou))
			.replace(/h/g, hou)
			.replace(/HH/g, (hou > 12? zpadding(hou - 12) : (hou < 1? 12 : zpadding(hou))))
			.replace(/H/g, (hou > 12? hou - 12 : (hou < 1? 12 : hou)))
			.replace(/mm/g, zpadding(min))
			.replace(/m/g, min)
			.replace(/tt/g, (hou >= 12? "pm" : "am"))
			.replace(/TT/g, (hou >= 12? "PM" : "AM"));
		return date_format;
	};

	var outputToInputObject = function($picker) {
		var $inp = getPickersInputObject($picker);
		if ($inp == null) {
			return;
		}
		var date = getPickedDate($picker);
		var locale = $picker.data("locale");
		var format = getDateFormat($picker.data("dateFormat"), locale, $picker.data('dateOnly'), $picker.data('timeOnly'));
		var old = $inp.val();
		$inp.val(getFormattedDate(date, format));
		if (old != $inp.val()) { // only trigger if it actually changed to avoid a nasty loop condition
			$inp.trigger("change");
		}
	};

	var getShownDate = function($obj) {
		var $picker = getParentPickerObject($obj);
		return $picker.data("shownDate");
	};

	var getPickedDate = function($obj) {
		var $picker = getParentPickerObject($obj);
		return $picker.data("pickedDate");
	};

	var zpadding = function(num) {
		num = ("0" + num).slice(-2);
		return num;
	};

	var draw_date = function($picker, option, date) {
		//console.log("draw_date - " + date.toString());
		draw($picker, option, date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes());
	};
	var translate = function(locale, s) {
		if (typeof lang[locale][s] !== "undefined"){
			return lang[locale][s];
		}
		return lang.en[s];
	};
	var draw = function($picker, option, year, month, day, hour, min) {

		var date = new Date();

		if (hour != null) {
			date = new Date(year, month, day, hour, min, 0);
		} else if (year != null) {
			date = new Date(year, month, day);
		} else {
			date = new Date();
		}

		/* Read options */
		var isTodayButton = $picker.data("todayButton");
		var isCloseButton = $picker.data("closeButton");
		var isScroll = option.isAnim; /* It same with isAnim */
		if($picker.data("timelistScroll") === false) {// If disabled by user option.
			isScroll = false;
		}

		var isAnim = option.isAnim;
		if($picker.data("animation") === false){ // If disabled by user option.
			isAnim = false;
		}

		var isFutureOnly = $picker.data("futureOnly");
		var minDate = $picker.data("minDate");
		var maxDate = $picker.data("maxDate");

		var isOutputToInputObject = option.isOutputToInputObject;
		var keepPickedDate = option.keepPickedDate;
		if (typeof keepPickedDate === "undefined") keepPickedDate = false;

		var minuteInterval = $picker.data("minuteInterval");
		var firstDayOfWeek = $picker.data("firstDayOfWeek");

		var allowWdays = $picker.data("allowWdays");
		if (allowWdays == null || isObj('Array', allowWdays) === false || allowWdays.length <= 0) {
			allowWdays = null;
		}

		var minTime = $picker.data("minTime");
		var maxTime = $picker.data("maxTime");

		/* Check a specified date */
		var todayDate = new Date();
		if (isFutureOnly) {
			if (date.getTime() < todayDate.getTime()) { // Already passed
				date.setTime(todayDate.getTime());
			}
		}
		if(allowWdays != null && allowWdays.length <= 6) {
			while (true) {
				if ($.inArray(date.getDay(), allowWdays) == -1) { // Unallowed wday
					// Slide a date
					date.setDate(date.getDate() + 1);
				} else {
					break;
				}
			}
		}

		/* Read locale option */
		var locale = $picker.data("locale");
		if (!lang.hasOwnProperty(locale)) {
			locale = 'en';
		}

		/* Calculate dates */
		var firstWday = new Date(date.getFullYear(), date.getMonth(), 1).getDay() - firstDayOfWeek;
		var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
		var beforeMonthLastDay = new Date(date.getFullYear(), date.getMonth(), 0).getDate();
		var dateBeforeMonth = new Date(date.getFullYear(), date.getMonth(), 0);
		var dateNextMonth = new Date(date.getFullYear(), date.getMonth() + 2, 0);
		var isCurrentYear = todayDate.getFullYear() == date.getFullYear();
		var isCurrentMonth = isCurrentYear && todayDate.getMonth() == date.getMonth();
		var isCurrentDay = isCurrentMonth && todayDate.getDate() == date.getDate();
		var isNextYear = (todayDate.getFullYear() + 1 == date.getFullYear());
		var isNextMonth = (isCurrentYear && todayDate.getMonth() + 1 == date.getMonth()) ||
			(isNextYear && todayDate.getMonth() === 11 && date.getMonth() === 0);
		var isPastMonth = false;
		if (date.getFullYear() < todayDate.getFullYear() || (isCurrentYear && date.getMonth() < todayDate.getMonth())) {
			isPastMonth = true;
		}

		/* Collect each part */
		var $header = $picker.children('.datepicker_header');
		var $inner = $picker.children('.datepicker_inner_container');
		var $calendar = $picker.children('.datepicker_inner_container').children('.datepicker_calendar');
		var $table = $calendar.children('.datepicker_table');
		var $timelist;
		if ($picker.children('.datepicker_inner_container').children('.datepicker_timelist.scroll-content').length) {
			$timelist = $picker.children('.datepicker_inner_container').children('.datepicker_timelist.scroll-content');
		} else {
			$timelist = $picker.children('.datepicker_inner_container').children('.datepicker_timelist');
		}

		/* Grasp a point that will be changed */
		var changePoint = "";
		var oldDate = getPickedDate($picker);
		if(oldDate != null){
			if(oldDate.getMonth() != date.getMonth() || oldDate.getDate() != date.getDate()){
				changePoint = "calendar";
			} else if (oldDate.getHours() != date.getHours() || oldDate.getMinutes() != date.getMinutes()){
				if(date.getMinutes() === 0 || date.getMinutes() % minuteInterval === 0){
					changePoint = "timelist";
				}
			}
		}

		/* Save newly date to Picker data */
		if (keepPickedDate === false) {
			$($picker).data("pickedDate", date);
		}
		$($picker).data("shownDate", date);

		/* Fade-out animation */
		if (isAnim === true) {
			if(changePoint == "calendar"){
				$calendar.stop().queue([]);
				$calendar.fadeTo("fast", 0.8);
			}else if(changePoint == "timelist"){
				$timelist.stop().queue([]);
				$timelist.fadeTo("fast", 0.8);
			}
		}
		/* Remind timelist scroll state */
		var drawBefore_timeList_scrollTop = $timelist.scrollTop();

		/* New timelist  */
		var timelist_activeTimeCell_offsetTop = -1;

		/* Header ----- */
		$header.children().remove();

		var cDate =  new Date(date.getTime());
		cDate.setMinutes(59);
		cDate.setHours(23);
		cDate.setSeconds(59);
		cDate.setDate(0); // last day of previous month

		var $link_before_month = null;
		if ((!isFutureOnly || !isCurrentMonth) && ((minDate == null) || (minDate < cDate.getTime()))
		) {
			$link_before_month = $('<a>');
			$link_before_month.text('');
			$link_before_month.prop('alt', translate(locale,'prevMonth'));
			$link_before_month.prop('title', translate(locale,'prevMonth') );
			$link_before_month.prop('class', 'icon-prevmonth' );
			$link_before_month.click(function() {
				beforeMonth($picker);
			});
			$picker.data('stateAllowBeforeMonth', true);
		} else {
			$picker.data('stateAllowBeforeMonth', false);
		}

		cDate.setMinutes(0);
		cDate.setHours(0);
		cDate.setSeconds(0);
		cDate.setDate(1); // First day of next month
		cDate.setMonth(date.getMonth() + 1);

		var $now_month = $('<span>');
		$now_month.text(date.getFullYear() + " " + translate(locale, 'sep') + " " + translate(locale, 'months')[date.getMonth()]);

		var $link_next_month = null;
		if ((maxDate == null) || (maxDate > cDate.getTime())) {
			$link_next_month = $('<a>');
			$link_next_month.text('');
			$link_next_month.prop('alt', translate(locale,'nextMonth'));
			$link_next_month.prop('title', translate(locale,'nextMonth'));
			$link_next_month.prop('class', 'icon-nextmonth' );
			$link_next_month.click(function() {
				nextMonth($picker);
			});
		}

		if (isTodayButton) {
			var $link_today = $('<a><div/></a>');
			$link_today.addClass('icon-home');
			$link_today.prop('alt', translate(locale,'today'));
			$link_today.prop('title', translate(locale,'today'));
			$link_today.click(function() {
				setToNow($picker);
			});
			$header.append($link_today);
		}
		if (isCloseButton) {
			var $link_close = $('<a><div/></a>');
			$link_close.addClass('icon-close');
			$link_close.prop('alt', translate(locale,'close'));
			$link_close.prop('title', translate(locale,'close'));
			$link_close.click(function() {
				setTimeout(function() {
					$picker.hide();
				}, 300);

				$picker.removeClass('datepicker_active');
			});
			$header.append($link_close);
		}

		if ($link_before_month != null) {
			$header.append($link_before_month);
		}
		$header.append($now_month);
		if ($link_next_month != null) {
			$header.append($link_next_month);
		}

		/* Calendar > Table ----- */
		$table.children().remove();
		var $tr = $('<tr>');
		$table.append($tr);

		/* Output wday cells */
		var firstDayDiff = 7 + firstDayOfWeek;
		var daysOfWeek = translate(locale,'days');
		var $td;
		for (var i = 0; i < 7; i++) {
			$td = $('<th>');
			$td.text(daysOfWeek[((i + firstDayDiff) % 7)]);
			$tr.append($td);
		}

		/* Output day cells */
		var cellNum = Math.ceil((firstWday + lastDay) / 7) * 7;
		i = 0;
		if(firstWday < 0){
			i = -7;
		}
		var realDayObj =  new Date(date.getTime());
		realDayObj.setHours(0);
		realDayObj.setMinutes(0);
		realDayObj.setSeconds(0);
		var pickedDate = getPickedDate($picker);
		var shownDate = getShownDate($picker);
		for (var zz = 0; i < cellNum; i++) {
			var realDay = i + 1 - firstWday;

			var isPast = isPastMonth ||
				(isCurrentMonth && realDay < todayDate.getDate()) ||
				(isNextMonth && firstWday > i && (beforeMonthLastDay + realDay) < todayDate.getDate());

			if (i % 7 === 0) {
				$tr = $('<tr>');
				$table.append($tr);
			}

			$td = $('<td>');
			$td.data("day", realDay);

			$tr.append($td);

			if (firstWday > i) {/* Before months day */
				$td.text(beforeMonthLastDay + realDay);
				$td.addClass('day_another_month');
				$td.data("dateStr", dateBeforeMonth.getFullYear() + "/" + (dateBeforeMonth.getMonth() + 1) + "/" + (beforeMonthLastDay + realDay));
				realDayObj.setDate(beforeMonthLastDay + realDay);
				realDayObj.setMonth(dateBeforeMonth.getMonth() );
				realDayObj.setYear(dateBeforeMonth.getFullYear() );
			} else if (i < firstWday + lastDay) {/* Now months day */
				$td.text(realDay);
				$td.data("dateStr", (date.getFullYear()) + "/" + (date.getMonth() + 1) + "/" + realDay);
				realDayObj.setDate( realDay );
				realDayObj.setMonth( date.getMonth()  );
				realDayObj.setYear( date.getFullYear() );
			} else {/* Next months day */
				$td.text(realDay - lastDay);
				$td.addClass('day_another_month');
				$td.data("dateStr", dateNextMonth.getFullYear() + "/" + (dateNextMonth.getMonth() + 1) + "/" + (realDay - lastDay));
				realDayObj.setDate( realDay - lastDay );
				realDayObj.setMonth( dateNextMonth.getMonth() );
				realDayObj.setYear( dateNextMonth.getFullYear() );
			}

			/* Check a wday */
			var wday = ((i + firstDayDiff) % 7);
			if(allowWdays != null) {
				if ($.inArray(wday, allowWdays) == -1) {
					$td.addClass('day_in_unallowed');
					continue; // Skip
				}
			} else if (wday === 0) {/* Sunday */
				$td.addClass('wday_sun');
			} else if (wday == 6) {/* Saturday */
				$td.addClass('wday_sat');
			}

			/* Set a special mark class */
			if (shownDate.getFullYear() == pickedDate.getFullYear() && shownDate.getMonth() == pickedDate.getMonth() && realDay == pickedDate.getDate()) { /* selected day */
				$td.addClass('active');
			}

			if (isCurrentMonth && realDay == todayDate.getDate()) { /* today */
				$td.addClass('today');
			}

			var realDayObjMN =  new Date(realDayObj.getTime());
			realDayObjMN.setHours(23);
			realDayObjMN.setMinutes(59);
			realDayObjMN.setSeconds(59);

			if (
				// compare to 23:59:59 on the current day (if MIN is 1pm, then we still need to show this day
			((minDate != null) && (minDate > realDayObjMN.getTime())) || ((maxDate != null) && (maxDate < realDayObj.getTime())) // compare to 00:00:00
			) { // Out of range day
				$td.addClass('out_of_range');
			} else if (isFutureOnly && isPast) { // Past day
				$td.addClass('day_in_past');
			} else {
				/* Set event-handler to day cell */
				$td.click(function(ev) {
					ev.stopPropagation();

					$(this).addClass('active');

					var $picker = getParentPickerObject($(this));
					var targetDate = new Date($(this).data("dateStr"));
					var selectedDate = getPickedDate($picker);
					draw($picker, {
						"isAnim": false,
						"isOutputToInputObject": true
					}, targetDate.getFullYear(), targetDate.getMonth(), targetDate.getDate(), selectedDate.getHours(), selectedDate.getMinutes());

					// Generate the handler of a picker
					var $input = $(this);
					var handler = new PickerHandler($picker, $input);

					// Call a event-hanlder for onSelect
					var func = $picker.data('onSelect');
					if (func != null) {
						func(handler, targetDate);
					}

					if ($picker.data("dateOnly") === true && $picker.data("isInline") === false && $picker.data("closeOnSelected")){
						// Close a picker
						ActivePickerId = -1;
						setTimeout(function() {
							$picker.hide();
						}, 300);

						$picker.removeClass('datepicker_active');
					}
				});

			}

			/* ---- */
		}

		if ($picker.data('timeOnly') === true) {
			$picker.addClass('datepicker-timeonly');
			$calendar.css("display", "none");
			$now_month.css("display", "none");
			if ($link_next_month != null)
				$link_next_month.css("display", "none");
			if ($link_before_month != null)
				$link_before_month.css("display", "none");
		}

		if ($picker.data("dateOnly") === true) {
			/* dateOnly mode */
			$picker.addClass('datepicker-dateonly');
			$timelist.css("display", "none");
		} else {
			/* Timelist ----- */

			/* Set height to Timelist (Calendar innerHeight - Calendar padding) */
			//if ($calendar.find('.datepicker_table').innerHeight() > 0) {
			//	$timelist.css("height", $calendar.find('.datepicker_table').innerHeight() - 5 + 'px');
			//}

			if ($timelist.find('.scroll-content').length) {
				// Go deeper
				$timelist = $timelist.find('.scroll-content');
			}
			$timelist.children().remove();

			realDayObj =  new Date(date.getTime());

			/* Output time cells */
			var hour_ = minTime[0];
			var min_ = minTime[1];

			while( hour_*100+min_ < maxTime[0]*100+maxTime[1] ){

				var $o = $('<div>');
				var is_past_time = hour_ < todayDate.getHours() || (hour_ == todayDate.getHours() && min_ < todayDate.getMinutes());
				var is_past = isCurrentDay && is_past_time;

				$o.addClass('timelist_item');
				var oText = "";
				if($picker.data("amPmInTimeList")){
					oText = /*zpadding*/(hour_ > 12? hour_ - 12 : (hour_ < 1? 12 : hour_));
					oText += ":" + zpadding(min_);
					oText += (hour_ >= 12? "PM" : "AM");
				} else {
					oText = zpadding(hour_) + ":" + zpadding(min_);
				}
				$o.text(oText);
				$o.data("hour", hour_);
				$o.data("min", min_);

				$timelist.append($o);

				realDayObj.setHours(hour_);
				realDayObj.setMinutes(min_);

				if (
					((minDate != null) && (minDate > realDayObj.getTime())) || ((maxDate != null) && (maxDate < realDayObj.getTime()))
				) { // Out of range cell
					$o.addClass('out_of_range');
				} else if (isFutureOnly && is_past) { // Past cell
					$o.addClass('time_in_past');
				} else { // Normal cell
					/* Set event handler to time cell */
					$o.click(function(ev) {
						ev.stopPropagation();

						$(this).addClass('active');

						var $picker = getParentPickerObject($(this));
						var date = getPickedDate($picker);
						var hour = $(this).data("hour");
						var min = $(this).data("min");
						draw($picker, {
							"isAnim": false,
							"isOutputToInputObject": true
						}, date.getFullYear(), date.getMonth(), date.getDate(), hour, min);

						if ($picker.data("isInline") === false && $picker.data("closeOnSelected")){
							// Close a picker
							ActivePickerId = -1;
							setTimeout(function() {
								$picker.hide();
							}, 300);

							$picker.removeClass('datepicker_active');
						}
					});
				}

				if (hour_ == date.getHours() && min_ == date.getMinutes()) { /* selected time */
					$o.addClass('active');
					timelist_activeTimeCell_offsetTop = $o.offset().top;
				}

				min_ += minuteInterval;
				if (min_ >= 60){
					min_ = min_ - 60;
					hour_++;
				}

			}

			/* Scroll the timelist */
			if(isScroll === true){
				/* Scroll to new active time-cell position */
				$timelist.scrollTop(timelist_activeTimeCell_offsetTop - $timelist.offset().top);
			}else{
				/* Scroll to position that before redraw. */
				$timelist.scrollTop(drawBefore_timeList_scrollTop);
			}
		}

		/* Fade-in animation */
		if (isAnim === true) {
			if (changePoint == 'calendar' && $picker.data('timeOnly') === false) {
				$calendar.fadeTo('fast', 1.0);
			} else if (changePoint == 'timelist' && $picker.data('dateOnly') === false) {
				$timelist.fadeTo('fast', 1.0);
			}
		}

		/* Output to InputForm */
		if (isOutputToInputObject === true) {
			outputToInputObject($picker);
		}

		var $inp = getPickersInputObject($picker);
		var handler = new PickerHandler($picker, $inp);
		handler._relocate();
	};

	/* Check for object type */
	var isObj = function(type, obj) {
		/* http://qiita.com/Layzie/items/465e715dae14e2f601de */
		var clas = Object.prototype.toString.call(obj).slice(8, -1);
		return obj !== undefined && obj !== null && clas === type;
	};

	var init = function($obj, opt) {
		/* Container */
		var $picker = $('<div>');

		$picker.destroy = function() {
			window.alert('destroy!');
		};

		$picker.addClass('datepicker');
		$obj.append($picker);

		/* Set current date */
		if(!opt.current) {
			opt.current = new Date();
		} else {
			var format = getDateFormat(opt.dateFormat, opt.locale, opt.dateOnly, opt.timeOnly);
			var date = parseDate(opt.current, format);
			if (date) {
				opt.current = date;
			} else {
				opt.current = new Date();
			}
		}

		/* Set options data to container object  */
		if (opt.inputObjectId != null) $picker.data("inputObjectId", opt.inputObjectId);
		if (opt.timeOnly) opt.todayButton = false;
		$picker.data("timeOnly", opt.timeOnly);
		$picker.data("dateOnly", opt.dateOnly);
		$picker.data("pickerId", PickerObjects.length);
		$picker.data("dateFormat", opt.dateFormat);
		$picker.data("locale", opt.locale);
		$picker.data("firstDayOfWeek", opt.firstDayOfWeek);
		$picker.data("animation", opt.animation);
		$picker.data("closeOnSelected", opt.closeOnSelected);
		$picker.data("timelistScroll", opt.timelistScroll);
		$picker.data("calendarMouseScroll", opt.calendarMouseScroll);
		$picker.data("todayButton", opt.todayButton);
		$picker.data("closeButton", opt.closeButton);
		$picker.data('futureOnly', opt.futureOnly);
		$picker.data('onShow', opt.onShow);
		$picker.data('onHide', opt.onHide);
		$picker.data('onSelect', opt.onSelect);
		$picker.data('onInit', opt.onInit);
		$picker.data('allowWdays', opt.allowWdays);

		if(opt.amPmInTimeList === true){
			$picker.data('amPmInTimeList', true);
		} else {
			$picker.data('amPmInTimeList', false);
		}

		var minDate = Date.parse(opt.minDate);
		if (isNaN(minDate)) { // invalid date?
			$picker.data('minDate', null); // set to null
		} else {
			$picker.data('minDate', minDate);
		}

		var maxDate = Date.parse(opt.maxDate);
		if (isNaN(maxDate)) { // invalid date?
			$picker.data('maxDate', null);  // set to null
		} else {
			$picker.data('maxDate', maxDate);
		}
		$picker.data("state", 0);

		if( 5 <= opt.minuteInterval && opt.minuteInterval <= 30 ){
			$picker.data("minuteInterval", opt.minuteInterval);
		} else {
			$picker.data("minuteInterval", 30);
		}
		opt.minTime = opt.minTime.split(':');
		opt.maxTime = opt.maxTime.split(':');

		if(! ((opt.minTime[0] >= 0 ) && (opt.minTime[0] <24 ))){
			opt.minTime[0]="00";
		}
		if(! ((opt.maxTime[0] >= 0 ) && (opt.maxTime[0] <24 ))){
			opt.maxTime[0]="23";
		}
		if(! ((opt.minTime[1] >= 0 ) && (opt.minTime[1] <60 ))){
			opt.minTime[1]="00";
		}
		if(! ((opt.maxTime[1] >= 0 ) && (opt.maxTime[1] <24 ))){
			opt.maxTime[1]="59";
		}
		opt.minTime[0]=parseInt(opt.minTime[0], 10); // parse as decimal number
		opt.minTime[1]=parseInt(opt.minTime[1], 10);
		opt.maxTime[0]=parseInt(opt.maxTime[0], 10);
		opt.maxTime[1]=parseInt(opt.maxTime[1], 10);
		$picker.data('minTime', opt.minTime);
		$picker.data('maxTime', opt.maxTime);

		/* Header */
		var $header = $('<div>');
		$header.addClass('datepicker_header');
		$picker.append($header);
		/* InnerContainer*/
		var $inner = $('<div>');
		$inner.addClass('datepicker_inner_container');
		$picker.append($inner);
		/* Calendar */
		var $calendar = $('<div>');
		$calendar.addClass('datepicker_calendar');
		var $table = $('<table>');
		$table.addClass('datepicker_table');
		$calendar.append($table);
		$inner.append($calendar);
		/* Timelist */
		var $timelist = $('<div>');
		$timelist.addClass('datepicker_timelist');
		$inner.append($timelist);

		/* Set event-handler to calendar */
		if (opt.calendarMouseScroll) {
			if (window.sidebar) { // Mozilla Firefox
				$calendar.bind('DOMMouseScroll', function(e){ // Change a month with mouse wheel scroll for Fx
					var $picker = getParentPickerObject($(this));

					// up,left [delta < 0] down,right [delta > 0]
					var delta = e.originalEvent.detail;
					/*
					 // this code need to be commented - it's seems to be unnecessary
					 // normalization (/3) is not needed as we move one month back or forth
					 if(e.originalEvent.axis !== undefined && e.originalEvent.axis == e.originalEvent.HORIZONTAL_AXIS){
					 e.deltaX = delta;
					 e.deltaY = 0;
					 } else {
					 e.deltaX = 0;
					 e.deltaY = delta;
					 }
					 e.deltaX /= 3;
					 e.deltaY /= 3;
					 */
					if(delta > 0) {
						nextMonth($picker);
					} else {
						beforeMonth($picker);
					}
					return false;
				});
			} else { // Other browsers
				$calendar.bind('mousewheel', function(e){ // Change a month with mouse wheel scroll
					var $picker = getParentPickerObject($(this));
					// up [delta > 0] down [delta < 0]
					if(e.originalEvent.wheelDelta /120 > 0) {
						beforeMonth($picker);
					} else {
						nextMonth($picker);
					}
					return false;
				});
			}
		}

		PickerObjects.push($picker);

		draw_date($picker, {
			"isAnim": true,
			"isOutputToInputObject": opt.autodateOnStart
		}, opt.current);

		/* Amethyst content */
		// Add scrollbar
		if($picker.find('.datepicker_timelist').length && !$picker.find('.scroll-content').length) {
			// Initialize scrollbar
			$picker.find('.datepicker_timelist').removeClass('scroll-wrapper');
			$picker.find('.datepicker_timelist').scrollbar({ignoreMobile: false});
		}
		// Add options to picker object
		$picker.data('opt', opt);
		/* /Amethyst content */
	};

	var getDefaults = function() {
		return {
			"current": null,
			"dateFormat": "default",
			"locale": "ru",
			"animation": false,
			"minuteInterval": 30,
			"firstDayOfWeek": 1,
			"closeOnSelected": true,
			"timelistScroll": true,
			"calendarMouseScroll": false,
			"todayButton": true,
			"closeButton": true,
			"dateOnly": false,
			"timeOnly": false,
			"futureOnly": false,
			"minDate" : null,
			"maxDate" : null,
			"autodateOnStart": true,
			"minTime":"00:00",
			"maxTime":"23:59",
			"onShow": null,
			"onHide": null,
			"onSelect": null,
			"allowWdays": null,
			"amPmInTimeList": false,
			"externalLocale": null,
			// Amethyst content
			"invertedClass": "inverted",
			"invertedParents": true
			// end of Amethyst content
		};
	};

	/**
	 * Initialize dtpicker
	 */
	$.fn.dtpicker = function(config) {
		var date = new Date();
		var defaults = getDefaults();

		if(typeof config === "undefined" || config.closeButton !== true){
			defaults.closeButton = false;
		}

		defaults.inputObjectId = undefined;
		var options = $.extend(defaults, config);

		return this.each(function(i) {
			init($(this), options);
		});
	};

	/**
	 * Initialize dtpicker, append to Text input field
	 * */
	$.fn.appendDtpicker = function(config) {
		var date = new Date();
		var defaults = getDefaults();

		if(typeof config !== "undefined" && config.inline === true && config.closeButton !== true){
			defaults.closeButton = false;
		}

		defaults.inline = false;
		var options = $.extend(defaults, config);

		if (options.externalLocale != null) {
			lang = $.extend(lang, options.externalLocale);
		}

		return this.each(function(i) {
			/* Checking exist a picker */
			var input = this;
			if(0 < $(PickerObjects[$(input).data('pickerId')]).length) {
				return;
			}

			/* Add input-field with inputsObjects array */
			var inputObjectId = InputObjects.length;
			InputObjects.push(input);

			options.inputObjectId = inputObjectId;

			/* Current date */
			var date, strDate, strTime;
			if($(input).val() != null && $(input).val() !== ""){
				options.current = $(input).val();
			}

			/* Make parent-div for picker */
			var $d = $('<div>');
			if(options.inline){ // Inline mode
				$d.insertAfter(input);
			} else { // Float mode
				$d.css("position","absolute");
				$('body').append($d);
			}

			/* Initialize picker */

			var pickerId = PickerObjects.length;

			var $picker_parent = $($d).dtpicker(options); // call dtpicker() method

			var $picker = $picker_parent.children('.datepicker');

			/* Link input-field with picker*/
			$(input).data('pickerId', pickerId);

			/* Set event handler to input-field */

			$(input).keyup(function() {
				var $input = $(this);
				var $picker = $(PickerObjects[$input.data('pickerId')]);
				if ($input.val() != null && (
					$input.data('beforeVal') == null ||
					( $input.data('beforeVal') != null && $input.data('beforeVal') != $input.val())	)
				) { /* beforeValue == null || beforeValue != nowValue  */
					var format = getDateFormat($picker.data('dateFormat'), $picker.data('locale'), $picker.data('dateOnly'), $picker.data('timeOnly'));
					var date = parseDate($input.val(), format);
					//console.log("dtpicker - inputKeyup - format: " + format + ", date: " + $input.val() + " -> " + date);
					if (date) {
						draw_date($picker, {
							"isAnim":true,
							"isOutputToInputObject":false
						}, date);
					}
				}
				$input.data('beforeVal', $input.val());
			});

			$(input).change(function(){
				$(this).trigger('keyup');
			});

			var handler = new PickerHandler($picker, $(input));

			if(options.inline === true){
				/* inline mode */
				$picker.data('isInline',true);
			} else {
				/* float mode */
				$picker.data('isInline',false);
				$picker_parent.css({
					"zIndex": 100
				});
				$picker.css("width","auto");

				/* Hide this picker */
				$picker.hide();

				/* Set onClick event handler for input-field */
				$(input).on('click, focus',function(ev){
					ev.stopPropagation();
					var $input = $(this);
					var $picker = $(PickerObjects[$input.data('pickerId')]);

					// Generate the handler of a picker
					var handler = new PickerHandler($picker, $input);
					// Get the display state of a picker
					var is_showed = handler.isShow();
					if (!is_showed) {
						// Show a picker
						handler.show();
						// Call a event-hanlder
						var func = $picker.data('onShow');
						if (func != null) {
							func(handler);
						}
					}
				});

				// Set an event handler for resizing of a window
				(function(handler){
					$(window).resize(function(){
						handler._relocate();
					});
					$(window).scroll(function(){
						handler._relocate();
					});
				})(handler);
			}

			// Set an event handler for removing of an input-field
			$(input).bind('destroyed', function() {
				var $input = $(this);
				var $picker = $(PickerObjects[$input.data('pickerId')]);
				// Generate the handler of a picker
				var handler = new PickerHandler($picker, $input);
				// Destroy a picker
				handler.destroy();
			});

			// Call a event-handler
			var func = $picker.data('onInit');
			if (func != null) {
				console.log("dtpicker- Call the onInit handler");
				func(handler);
			}
		});
	};

	/**
	 * Handle a appended dtpicker
	 * */
	var methods = {
		show : function( ) {
			var $input = $(this);
			var $picker = $(PickerObjects[$input.data('pickerId')]);
			if ($picker != null) {
				var handler = new PickerHandler($picker, $input);
				// Show a picker
				handler.show();
			}
		},
		hide : function( ) {
			var $input = $(this);
			var $picker = $(PickerObjects[$input.data('pickerId')]);
			if ($picker != null) {
				var handler = new PickerHandler($picker, $input);
				// Hide a picker
				handler.hide();
			}
		},
		setDate : function( date ) {
			var $input = $(this);
			var $picker = $(PickerObjects[$input.data('pickerId')]);
			if ($picker != null) {
				var handler = new PickerHandler($picker, $input);
				// Set a date
				handler.setDate(date);
			}
		},
		setMinDate : function( date ) {
			var $input = $(this);
			var $picker = $(PickerObjects[$input.data('pickerId')]);
			if ($picker != null) {
				var handler = new PickerHandler($picker, $input);
				// Set a min date
				handler.setMinDate(date);
			}
		},
		setMaxDate : function( date ) {
			var $input = $(this);
			var $picker = $(PickerObjects[$input.data('pickerId')]);
			if ($picker != null) {
				var handler = new PickerHandler($picker, $input);
				// Set a max date
				handler.setMaxDate(date);
			}
		},
		getDate : function( ) {
			var $input = $(this);
			var $picker = $(PickerObjects[$input.data('pickerId')]);
			if ($picker != null) {
				var handler = new PickerHandler($picker, $input);
				// Get a date
				return handler.getDate();
			}
		},
		destroy : function( ) {
			var $input = $(this);
			var $picker = $(PickerObjects[$input.data('pickerId')]);
			if ($picker != null) {
				var handler = new PickerHandler($picker, $input);
				// Destroy a picker
				handler.destroy();
			}
		}
	};

	$.fn.handleDtpicker = function( method ) {
		if ( methods[method] ) {
			return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
		} else if ( typeof method === 'object' || ! method ) {
			return methods.init.apply( this, arguments );
		} else {
			$.error( 'Method ' +  method + ' does not exist on jQuery.handleDtpicker' );
		}
	};

	if (!window.console) { // Not available a console on this environment.
		window.console = {};
		window.console.log = function(){};
	}

	/* Define a special event for catch when destroy of an input-field. */
	$.event.special.destroyed = {
		remove: function(o) {
			if (o.handler) {
				o.handler.apply(this, arguments);
			}
		}
	};

	/* Set event handler to Body element, for hide a floated-picker */
	$(function() {
		$('body').click(function() {
			for (var i=0;i<PickerObjects.length;i++) {
				var $picker = $(PickerObjects[i]);
				if ($picker.data('inputObjectId') != null && !$picker.data('isInline') && $picker.css('display') != 'none') {
					/* if append input-field && float picker */

					// Check overlapping of cursor and picker
					if ($picker.is(':hover')) continue;

					// Check overlapping of cursor and input-field
					var $input = $(InputObjects[$picker.data('inputObjectId')]);
					if ($input.is(':focus')) continue;

					// Hide a picker
					var handler = new PickerHandler($picker, $input);
					handler.hide();

					// Call a event-hanlder
					var func = $picker.data('onHide');
					if (func != null) {
						console.log('dtpicker- Call the onHide handler');
						func(handler);
					}
				}
			}
		});
	});

})(jQuery);
// require "shards/fittext/_fittext.js"

// Offcanvas menu (pick one)
// require "shards/navigation_offcanvas_top/_navigation_offcanvas_top.js"
// require "shards/navigation_offcanvas_slideover/_navigation_offcanvas_slideover.js"
// require "shards/navigation_offcanvas_slide/_navigation_offcanvas_slide.js"

var regTimer;

(function($, undefined){

	// Local variables
	var resizeTimer;
	var $window = $(window);
	var $document = $(document);

	$('[data-toggle="tooltip"]').tooltip({
		container: 'body'
	});

	$('#register-modal-input-bd').appendDtpicker({
		"dateOnly": true
	});

	$('.dateonly-pick').appendDtpicker({
		"dateOnly": true
	});

	$('.masked-email').blur(function() {
		if($(this).val() != '') {
			var pattern = /^([a-z0-9_\.-])+@[a-z0-9-]+\.([a-z]{2,4}\.)?[a-z]{2,4}$/i;
			if(pattern.test($(this).val())){
				$(this).removeClass('error');
			} else {
				$(this).addClass('error');
			}
		} else {
			$(this).addClass('error');
		}
	});

	$('.required').blur(function() {
		if($(this).val() != '') {
			$(this).removeClass('error');
		} else {
			$(this).addClass('error');
		}
	});

	$('.masked-phone').mask('+7 (999) 999-99-99');

	//$('.masked-phone-card').mask('+7 (999) 999-99-99');
	$('.masked-phone-card').on('keydown', function(event) {

		var code = event.keyCode;
		var $this = $(this);
		if ($this.val() != '' && code == 8) {
			$this.val('');
			$this.mask('');
		} else {
			if ($this.val() == '') {
				if (code == '50') {
					$this.mask('999999');
				} else if(code == '56' || code == '187' || code == '55' || code == '57') {
					$this.mask('+7 (999) 999-99-99');
				}
			}
		}
	});

	// Swiper
	var mySwiper = new Swiper ('.swiper-container', {
		// Optional parameters
		slidesPerView: 1,
		speed: 470,
		autoplay: 4000,
		//freeMode: true,
		loop: true,
		keyboardControl: true,
		mousewheelControl: false,

		// Enable lazy loading
		preloadImages: false,
		lazyLoading: true,
		lazyLoadingInPrevNext: true,

		// If we need pagination
		pagination: '.swiper-pagination',
		paginationClickable: true,

		// Navigation arrows
		nextButton: '.swiper-button-next',
		prevButton: '.swiper-button-prev',
	});

	// Checkbox & radios replace (_icheck_checkboxes_radios.js)
	$('INPUT[type="checkbox"], INPUT[type="radio"]').icheck({
		autoInit: true,
		tap: true,
		checkboxClass: 'amethyst_checkbox',
		radioClass: 'amethyst_radio'
	});

	// Project scripts

	$document.on('click', '.star-holder A', function(event) {
		var $this = $(this);
		var data = $this.attr("data");
		var dtype = $this.attr('dtype');

		console.log($this.attr("data"));

		var method = "";
		var idname = "";
		if (dtype == "campaings") {
		    method = "GetSetClientCampaign";
		    idname = "CampaignID";
		} else if (dtype == "partners") {
		    method = "GetSetClientPartner";
            idname = "PartnerID"
		}

		$this.tooltip('hide');
		var $holder = $this.closest('.star-holder');

		if (!$holder.hasClass('star-unauth')) {
			if ($this.hasClass('star-half')) {
			    // Check
			    $holder.addClass('star-checked');
				//$holder.find('.star-full').tooltip('show');
			} else {
				// Uncheck
				$holder.removeClass('star-checked');
				//$holder.find('.star-empty').tooltip('show');
			}
		}

		$.ajax({
		    type: "POST",
		    url: '/API.aspx/'+method,
		    data: '{'+idname+':\'' + data + '\',Remove:false}',
		    contentType: "application/json; charset=utf-8",
		    dataType: "json",
		    success: function (data) {
		        var rs = JSON.parse(data.d);
		        if (rs.ErrorCode != 0) {
		            customAlert("Ошибка", rs.Message);
		        } else {
		        }
		    }
		});

		event.preventDefault();
	});

	$document.on('click', '.action-blocks-toggle', function() {
		var toggledClass = $(this).data('toggle-class');
		$('.' + toggledClass).toggle();
		return false;
	});

	$document.on('click', '.form-edit', function(event) {
		var $this = $(this);
		var $wrapper = $this.closest('.form-wrapper');

		$wrapper.addClass('form-editable');

		if ($this.hasClass('form-edit-switch')) {
			$this.removeClass('form-edit').addClass('form-cancel');
		}

		event.preventDefault();
	});

	$document.on('click', '.map-filter-line-self', function(event) {
		var $this = $(this);
		$this.toggleClass('active');

		event.preventDefault();
	});



	$document.on('click', '.form-save', function(event) {
		var $wrapper = $(this).closest('.form-wrapper');
		$wrapper.removeClass('form-editable').addClass('form-saved');
		var pb = $("#f6").val();
		var b = $("#f6").val().split(".");

		var lastname = $("#lname").val();
		var firstname = $("#fname").val();
		var middlename = $("#mname").val();
		var gender = $("#f4").val()*1;
		var haschildren = $("#f5").val();
		var birthday = b[1] + "." + b[0] + "." + b[2] + " 00:00:00";
		var bio = $("#bio").val();

		$.ajax({
		    type: "POST",
		    url: '/API.aspx/SaveProfile1',
		    data: '{Lastname:\'' + lastname + '\',Firstname:\'' + firstname + '\', Middlename:\'' + middlename + '\', Gender:\'' + gender + '\', Haschildren:\'' + haschildren + '\', Birthday:\'' + birthday + '\', Description:\'' + escape(bio) + '\'}',
		    contentType: "application/json; charset=utf-8",
		    dataType: "json",
		    success: function (data) {
		        var rs = JSON.parse(data.d);
		        console.log(rs);
		        if (rs.ErrorCode != 0) {
		            customAlert("Ошибка", rs.Message);
		        } else {
		            $("#ulname").text(lastname);
		            $("#ufname").text(firstname);
		            $("#umname").text(middlename);
		            switch(gender)
		            {
		                case -1:
		                    $("#ugender").text("Жен.");
		                    break;
		                case 1:
		                    $("#ugender").text("Муж.");
		                    break;
		                default:
		                    $("#ugender").text("-");
		                    break;
		            }
		            $("#uhaschildren").text((haschildren=='true')?"Да":"Нет");
		            $("#ubirthdate").text(pb);
		            $("#ubio").text(bio);
		            $("a.header-user-name").html("Зравствуйте, "+firstname);
		        }

		        //                    location.reload();
		    }
		});

		console.log(lastname);

		event.preventDefault();
	});

	$document.on('click', '#profile-password-change', function (event) {
	    $("#ucode").val("");
	    var $this = $(this);
	    var phone = $("#f7").val();
	    var phone_parsed = phone.replace("+7", "").replace(/\D/g, "");

	    var p1 = $("#change-client-pass1");
	    var p2 = $("#change-client-pass2");

	    var error = false;
	    if (p1.val() != p2.val())
	    {
	        error = true;
	        customAlert("Внимание!", "Пароли не совпадают!");
	    }
	    if (p1.val().length < 6) {
	        error = true;
	        customAlert("Внимание!", "Минимальная длина пароля 6 символов!");
	    }
	    if (!error) {

	        $.ajax({
	            type: "POST",
	            url: '/API.aspx/ChangeClientPassword',
	            data: '{Password:\''+escape(p1.val())+'\'}',
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (data) {
	            var rs = JSON.parse(data.d);
	            console.log(rs);
	            if (rs.ErrorCode != 0) {
	                customAlert("Ошибка", rs.Message);
	            } else {
	                var $wrapper = $($this).closest('.form-wrapper');
	                $wrapper.addClass('form-saved form-saved-first');

	            }

	            //                    location.reload();
	        }
	        });
	    }
		event.preventDefault();
	});

	$document.on('click', '.form-save-second', function(event) {
	    var $wrapper = $(this).closest('.form-wrapper');
		$wrapper.addClass('form-saved-second');
		var phone = $("#f7").val();
		var phone_parsed = phone.replace("+7", "").replace(/\D/g, "");
		var code = $("#ucode").val();
		$.ajax({
		    type: "POST",
		    url: '/API.aspx/SaveProfilePhone',
		    data: '{Phone:\'' + phone_parsed + '\',Code:\''+code+'\'}',
		    contentType: "application/json; charset=utf-8",
		    dataType: "json",
		    success: function (data) {
		        var rs = JSON.parse(data.d);
		        console.log(rs);
		        if (rs.ErrorCode != 0) {
		            customAlert("Ошибка", rs.Message);
		        } else {
		            $wrapper.removeClass('form-editable form-saved');
		            $("#uphone").html(phone);
		            if ($this.hasClass('form-edit-switch')) {
		                $this.addClass('form-edit').removeClass('form-cancel');
		            }

		        }
		        //                    location.reload();
		    }
		});

		event.preventDefault();
	});

	$document.on('click', '.form-switch A', function(event) {
		var $wrapper = $(this).closest('.form-switch');
		if ($(this).hasClass('switch-check')) {
		    $wrapper.addClass('form-switch-checked').trigger('classChange');
		} else {
		    $wrapper.removeClass('form-switch-checked').trigger('classChange');
		}
		switch ($wrapper.attr('id')) {
		    case 'profile-allow-sms':

		        $.ajax({
		            type: "POST",
		            url: '/API.aspx/AllowSMS',
		            data: '{allow:\'' + ($wrapper.hasClass('form-switch-checked')) + '\'}',
		            contentType: "application/json; charset=utf-8",
		            dataType: "json",
		            success: function (data) {
		                var rs = JSON.parse(data.d);
		                console.log(rs);
		                if (rs.ErrorCode != 0) {
		                    customAlert("Ошибка", rs.Message);
		                } else {
		                }
		                //                    location.reload();
		            }
		        });

		        break;
		    case 'profile-allow-email':
		        $.ajax({
		            type: "POST",
		            url: '/API.aspx/AllowEMAIL',
		            data: '{allow:\'' + ($wrapper.hasClass('form-switch-checked')) + '\'}',
		            contentType: "application/json; charset=utf-8",
		            dataType: "json",
		            success: function (data) {
		                var rs = JSON.parse(data.d);
		                console.log(rs);
		                if (rs.ErrorCode != 0) {
		                    customAlert("Ошибка", rs.Message);
		                } else {
		                }
		                //                    location.reload();
		            }
		        });
		        break;
		    default:
		        if ($wrapper.hasClass('personal-category-switch')) {
		            var cval = $wrapper.attr('data');
		            $.ajax({
		                type: "POST",
		                url: '/API.aspx/SetPreferences',
		                data: '{Set:\'' + ($wrapper.hasClass('form-switch-checked')) + '\',CategoryID:\''+cval+'\'}',
		                contentType: "application/json; charset=utf-8",
		                dataType: "json",
		                success: function (data) {
		                    var rs = JSON.parse(data.d);
		                    console.log(rs);
		                    if (rs.ErrorCode != 0) {
		                        customAlert("Ошибка", rs.Message);
		                    } else {
		                    }
		                    //                    location.reload();
		                }
		            });
                }
		        break;
		}
		event.preventDefault();
	});
    /*
	$document.on('click', '.filter-number', function(event) {
		$(this).toggleClass('filter-number-highlight');
		event.preventDefault();
	});

	$document.on('click', '.map-filter-line A', function(event) {
		$(this).closest('.map-filter-line').find('.filter-number').toggleClass('filter-number-highlight');
		//event.preventDefault();
	});
    */


	$document.on('click', '.form-cancel', function(event) {
		var $this = $(this);
		var $wrapper = $this.closest('.form-wrapper');
		$wrapper.removeClass('form-editable form-saved');
		var $wrapper = $this.closest('.form-saved-first');
		$wrapper.removeClass('form-saved form-saved-first');

		if ($this.hasClass('form-edit-switch')) {
			$this.addClass('form-edit').removeClass('form-cancel');
		}

		event.preventDefault();
	});

	$document.on('click', '#register-continue', function(event) {
	    var error = false;
		$(this).closest('FORM').find('.required').each(function() {
			if ($(this).val() == '') {
				$(this).addClass('error');
				error = true;
			} else {
				if ($(this).hasClass('masked-email')) {
					var pattern = /^([a-z0-9_\.-])+@[a-z0-9-]+\.([a-z]{2,4}\.)?[a-z]{2,4}$/i;
					if(pattern.test($(this).val())){
					    $(this).removeClass('error');

					} else {
						$(this).addClass('error');
						error = true;
					}
				} else {
					$(this).removeClass('error');
				}
			}
		});

		var p1 = $('#register-password');
		var p2 = $('#register-password2');

		if (p1.val() != p2.val() || p1.val().length < 6) {
		    console.log(p2.val().length);
		    //$(p1).addClass('error');
		    $(p2).addClass('error');
		    error = true;
		}

		var agree = $("#register-agree");
		if (agree.val() != "yes")
		{
		    $(agree).addClass('error');
		    error = true;
		}

		if (!error) {
		    $("#register-firstname").addClass("disabled").prop('disabled',true);
		    $("#register-middlename").addClass("disabled").prop('disabled', true);
		    $("#register-lastname").addClass("disabled").prop('disabled', true);
		    $("#register-phone").addClass("disabled").prop('disabled', true);
		    $("#register-email").addClass("disabled").prop('disabled', true);
//		    $("#havecard").addClass("disabled");

		    $('#havecard')[0].selectize.lock();


		    $("#register-cardnum").addClass("disabled").prop('disabled', true);
		    $("#register-modal-input-bd").addClass("disabled").prop('disabled', true);
//		    $("#register-gender").addClass("disabled");

		    $('#register-gender')[0].selectize.lock();


		    $("#register-password").addClass("disabled").prop('disabled', true);
		    $("#register-password2").addClass("disabled").prop('disabled', true);
//		    $("#register-agree").addClass("disabled");

		    $('#register-agree')[0].selectize.lock();


		    $('#register-step2').show();
		    var valthis = $(this).closest('FORM').find(".masked-phone").first().val();
		    var val = valthis.replace("+7","").replace(/\D/g, "");
		    /*
                SEND CODE
            */
		    
		    $("#register-continue").prop('disabled', true);
		    $.ajax({
                type: "POST",
                url: '/API.aspx/GetVerificationCode',
                data: '{Phone:\'' + val + '\'}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var rs = JSON.parse(data.d);
                    console.log(rs);
                    if (rs.ErrorCode != 0) {
                        customAlert("Ошибка", rs.Message);
                    }

//                    location.reload();
                }
            });
            

			initTimer();
		}
		event.preventDefault();
	});

	$document.on('change', '#havecard', function() {
		if ($(this).val() == 'yes') {
			$('#form-item-cardnum').show();
		} else {
			$('#form-item-cardnum').hide();
		}
	});

	$document.on('click', '.form-item-request .timer-button BUTTON', function(event) {
		$('.form-item-request .timer').show();
		$('.form-item-request .timer-button').hide();

		initTimer();
		event.preventDefault();
	});

	var initTimer = function() {
		clearInterval(regTimer);
		$('.form-item-request .timer SPAN').html(60);
		var curTime = 59;
		regTimer = setInterval(function() {
			$('.form-item-request .timer SPAN').html(curTime);
			curTime--;

			if (curTime == 0) {
				$('.form-item-request .timer').hide();
				$('.form-item-request .timer-button').show();

				clearInterval(timer);
			}
		}, 1000);
	};

	$('SELECT').selectize({
		create: false,
		persist: false
	});

	// ------------------------------
	// Resize & orientation change functions with 100ms timer

	function resizeElements() {
		// Some recalculations on resize

		// ...
	}
	$window.on('resize', function() {
		clearTimeout(resizeTimer);
		resizeTimer = setTimeout(resizeElements, 100);
	});
	// Bind orientation change
	$window.on('orientationchange', function() {
		resizeElements();
	});
	// Fire once at startup
	resizeElements();



})(jQuery);

function checkEmailFormat(email) {
    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    if (email.match(mailformat)) {
        return true;
    }
    return false;
}


Share = {
    vkontakte: function (purl, ptitle, pimg, text) {
        url = 'http://vk.com/share.php?';
        url += 'url=' + encodeURIComponent(purl);
        url += '&image=' + encodeURIComponent(pimg);
        url += '&title=' + encodeURIComponent(ptitle);
        url += '&noparse=true';
        //	url += '&description=' + encodeURIComponent(text);

        Share.popup(url);
    },
    odnoklassniki: function (purl, text) {
        url = 'http://www.odnoklassniki.ru/dk?st.cmd=addShare&st.s=1';
        url += '&st.comments=' + encodeURIComponent(text);
        url += '&st._surl=' + encodeURIComponent(purl);
        Share.popup(url);
    },
    facebook: function (purl, ptitle, pimg, text) {
        url = 'http://www.facebook.com/sharer/sharer.php?';
//        url += '&[ptitle]=' + encodeURIComponent(ptitle);
        url += 'u=' + encodeURIComponent(purl);
//        url += '&[pimage-url]=' + encodeURIComponent(pimg);
//        url += '&[psummary]=' + encodeURIComponent(text);
        Share.popup(url);
    },
    twitter: function (purl, ptitle, pimg, text, author) {
        url = 'https://twitter.com/intent/tweet?';
        url += 'text=' + encodeURIComponent(ptitle);
        url += ' ' + encodeURIComponent("\n" + author);
        url += ' ' + encodeURIComponent("\n" + purl);
        //		url += ' '    + encodeURIComponent("\n"+text);
        //		url += ' '    + encodeURIComponent("\n"+pimg);
        Share.popup(url);
    },
    mailru: function (purl, ptitle, pimg, text) {
        url = 'http://connect.mail.ru/share?';
        url += 'url=' + encodeURIComponent(purl);
        url += '&title=' + encodeURIComponent(ptitle);
        url += '&description=' + encodeURIComponent(text);
        url += '&imageurl=' + encodeURIComponent(pimg);
        Share.popup(url)
    },

    popup: function (url) {
        window.open(url, '', 'toolbar=0,status=0,width=626,height=436');
    }
};


