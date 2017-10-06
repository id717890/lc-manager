$(document).ready(function() {
     $('#subok').on('click', function(e){
         e.preventDefault();
         $('body').css('overflow', 'hidden');
         $('.modal-body-cofirm').css('display', 'flex');
         $('.overlay').css('display', 'block');
     });
    $('#suberr').on('click', function(e){
         e.preventDefault();
         $('body').css('overflow', 'hidden');
         $('.modal-body-err').css('display', 'flex');
         $('.overlay').css('display', 'block');
     });

     $('.modal_conform').on('click', function(e){
         e.preventDefault();
        $('body').css('overflow', 'auto'); 
         $('.modal-body-cofirm').css('display', 'none');
         $('.overlay').css('display', 'none');

         $('body').css('overflow', 'hidden');
         $('.modal-body-ok').css('display', 'flex');
         $('.overlay').css('display', 'block');
     });
     $('.modal_close, .overlay').click( function(){
         $('body').css('overflow', 'auto'); 
         $('.modal-body-cofirm').css('display', 'none');
         $('.overlay').css('display', 'none');
     });
    $('.modalclose').click( function(){
         $('body').css('overflow', 'auto'); 
         $('.modal-body-ok').css('display', 'none');
         $('.overlay').css('display', 'none');
     });
    $('.modclose').click( function(){
         $('body').css('overflow', 'auto'); 
         $('.modal-body-err').css('display', 'none');
         $('.overlay').css('display', 'none');
     });
});

(function ($) {
    var input_class = 'zbz-input-clearable',
        input_class_x = input_class + '__x',
        input_class_x_over = input_class + '__x-over',
        input_selector = '.' + input_class,
        input_selector_x = '.' + input_class_x,
        input_selector_x_over = '.' + input_class_x_over,
        event_main = input_class + '-init',
        event_names = [event_main, 'focus drop paste keydown keypress input change'].join(' '),
        btn_width = 13,
        btn_height = 13,
        btn_margin = 7;

    function tog(v) {
        return v ? 'addClass' : 'removeClass';
    }

    $(document).on(event_names, input_selector, function () {
        $(this)[tog(this.value)](input_class_x);
    });

    $(document).on('mousemove', input_selector_x, function (e) {
        var input = $(this),
            input_width = this.offsetWidth,
            input_height = this.offsetHeight,
            input_border_bottom = parseFloat(input.css('borderBottomWidth')),
            input_border_right = parseFloat(input.css('borderRightWidth')),
            input_border_left = parseFloat(input.css('borderLeftWidth')),
            input_border_top = parseFloat(input.css('borderTopWidth')),
            input_border_hr = input_border_left + input_border_right,
            input_border_vr = input_border_top + input_border_bottom,
            client_rect = this.getBoundingClientRect(),
            input_cursor_pos_x = e.clientX - client_rect.left,
            input_cursor_pos_y = e.clientY - client_rect.top,
            is_over_cross = true;

        is_over_cross = is_over_cross && (input_cursor_pos_x >= input_width - input_border_hr - btn_margin - btn_width);
        is_over_cross = is_over_cross && (input_cursor_pos_x <= input_width - input_border_hr - btn_margin);
        is_over_cross = is_over_cross && (input_cursor_pos_y >= (input_height - input_border_vr - btn_height) / 2);
        is_over_cross = is_over_cross && (input_cursor_pos_y <= (input_height - input_border_vr - btn_height) / 2 + btn_height);

        $(this)[tog(is_over_cross)](input_class_x_over);
    });

    $(document).on('click', input_selector_x_over, function () {
        $(this).removeClass([input_class_x, input_class_x_over].join(' ')).val('').trigger('input');
    });

    $(function () {
        $(input_selector).trigger(event_main);
    });

})(jQuery);