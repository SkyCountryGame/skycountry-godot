shader_type spatial;

uniform sampler2D base_albedo_tex;
//original texture

uniform sampler2D blend_albedo_tex;
//texture that will be sampled on top

varying vec2 base_uv;
varying vec2 blend_uv;
//define UV's in vertex shaders

void vertex() {
	base_uv = UV;
	blend_uv = UV;
}
//read from base & blend texture based on defined UV's

void fragment() {
	vec3 base_albedo = texture(base_albedo_tex, base_uv).rgb;
	vec3 blend_albedo = texture(blend_albedo_tex, blend_uv).rgb;

	float mask = 1.0;
	ALBEDO = mix(base_albedo, blend_albedo, mask);
	//mix between base albedo and blend albedo based on mask value

}
