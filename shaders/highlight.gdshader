shader_type spatial;
render_mode blend_add, cull_back, depth_draw_always;

//uniform float strength = 0.1;
uniform vec4 emission_color : source_color;
uniform float emission_amount : hint_range(0.0, 16.0) = 1.3;
uniform float rim_sharpness : hint_range(0.0, 16.0) = 2.25;

void fragment() {
	//vec3 color;
	float delta = (sin(TIME * 2.0) + 1.0) * 0.4;
	//color.r = delta;
	//color.g = delta;
	//color.b = delta;
	//ALBEDO = color;
	//ALPHA = strength;
	EMISSION = emission_color.rgb * emission_amount * delta;
	ALPHA = pow(1.0 - dot(NORMAL, VIEW), rim_sharpness) * emission_color.a;
}
